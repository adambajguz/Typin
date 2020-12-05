namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers;
    using TypinExamples.Infrastructure.WebWorkers.Common.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal.Messaging;

    public class WorkerInstanceManager : IWorker,
        ICommandHandler<RunProgramCommand, RunProgramResult>,
        ICommandHandler<CancelCommand>,
        ICommandHandler<DisposeCommand>
    {
        private readonly ulong _initCallId;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly ISerializer _serializer;

        private ServiceProvider? _serviceProvider;
        private IMessagingService? _messagingService;

        public ulong Id { get; }
        public bool IsDisposed { get; private set; }
        public bool IsInitialized { get; private set; }

        public WorkerInstanceManager(ulong id, ulong initCallId, ISerializer? serializer = null)
        {
            Id = id;

            _initCallId = initCallId;
            _serializer = serializer ?? new DefaultSerializer();
        }

        internal static void ConfigureCoreHandlers(IWorkerConfigurationBuilder builder)
        {
            builder.RegisterCommandHandler<RunProgramCommand, WorkerInstanceManager, RunProgramResult>();
            builder.RegisterCommandHandler<CancelCommand, WorkerInstanceManager>();
            builder.RegisterCommandHandler<DisposeCommand, WorkerInstanceManager>();
        }

        public void Start(string? startupType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(startupType))
                    throw new ArgumentException($"'{nameof(startupType)}' cannot be null or whitespace", nameof(startupType));

                //Create startup class
                Type type = Type.GetType(startupType) ?? throw new InvalidOperationException("Invalid startup class type.");
                IWorkerStartup startup = Activator.CreateInstance(type) as IWorkerStartup ?? throw new InvalidOperationException("Invalid startup class.");

                //Build configuration and service collection
                WorkerConfigurationBuilder configurationBuilder = new();
                startup.Configure(configurationBuilder);

                WorkerConfiguration configuration = configurationBuilder.Build();

                //Build DI container
                ServiceCollection serviceCollection = new ServiceCollection();
                startup.ConfigureServices(serviceCollection);

                serviceCollection.AddTransient(typeof(IWorkerProgram), configuration.ProgramType)
                                 .AddTransient(typeof(NotificationHandlerWrapper<>))
                                 .AddTransient(typeof(CommandHandlerWrapper<>))
                                 .AddTransient(typeof(CommandHandlerWrapper<,>))
                                 .AddSingleton<ICommandHandler<RunProgramCommand, RunProgramResult>>(this)
                                 .AddSingleton<ICommandHandler<CancelCommand>>(this)
                                 .AddSingleton<ICommandHandler<DisposeCommand>>(this)
                                 .AddSingleton(_serializer)
                                 .AddSingleton(configuration)
                                 .AddSingleton<IWorker>(this)
                                 .AddSingleton<IMessagingProvider, WorkerThreadMessagingProvider>()
                                 .AddSingleton<IMessagingService, WorkerMessagingService>()
                                 .AddSingleton(new WorkerIdAccessor(Id))
                                 .AddSingleton(new WorkerCancellationTokenAccessor(_cancellationTokenSource.Token))
                                 .AddScoped<HttpClient>();

                Type[] corePayloads = new[] { typeof(RunProgramCommand), typeof(CancelCommand), typeof(DisposeCommand) };

                foreach (MessageMapping mapping in configuration.MessageMappings.Values)
                {
                    if (corePayloads.Contains(mapping.PayloadType))
                        continue;

                    serviceCollection.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);
                }

                _serviceProvider = serviceCollection.BuildServiceProvider();

                //Confirm initialization
                IMessagingService messaging = _serviceProvider.GetRequiredService<IMessagingService>();
                _messagingService = messaging;

                IsInitialized = true;

                messaging.PostAsync(null, new Message<InitializeResult>
                {
                    Id = _initCallId,
                    WorkerId = Id,
                    TargetWorkerId = null,
                    Type = MessageTypes.FromWorker | MessageTypes.Result,
                    Payload = new InitializeResult()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task NotifyAsync<TNotification>(TNotification payload)
            where TNotification : INotification
        {
            if (_messagingService is null || !IsInitialized || !IsDisposed)
                throw new InvalidOperationException($"{nameof(NotifyAsync)}<{typeof(TNotification).Name}>: Worker not initialized.");

            await _messagingService.NotifyAsync(null, payload);
        }

        public async Task CallCommandAsync<TCommand>(TCommand payload)
            where TCommand : ICommand
        {
            if (_messagingService is null || !IsInitialized || !IsDisposed)
                throw new InvalidOperationException($"{nameof(NotifyAsync)}<{typeof(TCommand).Name}>: Worker not initialized.");

            await _messagingService.CallCommandAsync<TCommand, CommandFinished>(null, payload);
        }

        public async Task<TResult> CallCommandAsync<TCommand, TResult>(TCommand payload)
            where TCommand : ICommand<TResult>
        {
            if (_messagingService is null || !IsInitialized || !IsDisposed)
                throw new InvalidOperationException($"{nameof(CallCommandAsync)}<{typeof(TCommand).Name}, {typeof(TResult).Name}>: Worker not initialized.");

            return await _messagingService.CallCommandAsync<TCommand, TResult>(null, payload);
        }

        public Task<int> RunAsync()
        {
            throw new NotImplementedException();
        }

        public Task CancelAsync()
        {
            _cancellationTokenSource.Cancel();

            return Task.CompletedTask;
        }

        public Task CancelAsync(TimeSpan delay)
        {
            _cancellationTokenSource.CancelAfter(delay);

            return Task.CompletedTask;
        }

        #region Core Handlers
        async ValueTask<RunProgramResult> ICommandHandler<RunProgramCommand, RunProgramResult>.HandleAsync(RunProgramCommand request, IWorker worker, CancellationToken cancellationToken)
        {
            _ = _serviceProvider ?? throw new InvalidOperationException($"{nameof(RunProgramCommand)} Handler: Worker not initialized.");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IWorkerProgram workerProgram = scope.ServiceProvider.GetRequiredService<IWorkerProgram>();
                int exitCode = await workerProgram.Main(cancellationToken);

                return new RunProgramResult
                {
                    ExitCode = exitCode
                };
            }
        }

        ValueTask<CommandFinished> ICommandHandler<CancelCommand, CommandFinished>.HandleAsync(CancelCommand request, IWorker worker, CancellationToken cancellationToken)
        {
            if (request.Delay == TimeSpan.Zero)
                _cancellationTokenSource.Cancel();
            else
                _cancellationTokenSource.CancelAfter(request.Delay);

            return CommandFinished.Task;
        }

        async ValueTask<CommandFinished> ICommandHandler<DisposeCommand, CommandFinished>.HandleAsync(DisposeCommand request, IWorker worker, CancellationToken cancellationToken)
        {
            await DisposeAsync();

            return CommandFinished.Instance;
        }

        public ValueTask DisposeAsync()
        {
            if (!IsDisposed)
            {
                _cancellationTokenSource.Cancel();
                _serviceProvider?.Dispose();
                _cancellationTokenSource.Dispose();
                IsDisposed = true;
            }

            return default;
        }
        #endregion
    }
}
