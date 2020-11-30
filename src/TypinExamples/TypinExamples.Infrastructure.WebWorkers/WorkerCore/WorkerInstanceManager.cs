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
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal.Messaging;

    public class WorkerInstanceManager :
        ICommandHandler<RunProgramPayload, ProgramFinishedPayload>,
        ICommandHandler<CancelPayload>,
        ICommandHandler<DisposePayload>
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly ISerializer _serializer;

        private ServiceProvider? _serviceProvider;

        public ulong Id { get; }

        public WorkerInstanceManager(ulong id, ISerializer? serializer = null)
        {
            Id = id;

            _serializer = serializer ?? new DefaultSerializer();
        }

        internal static void ConfigureCoreHandlers(IWorkerConfigurationBuilder builder)
        {
            builder.RegisterCommandHandler<RunProgramPayload, WorkerInstanceManager, ProgramFinishedPayload>();
            builder.RegisterCommandHandler<CancelPayload, WorkerInstanceManager>();
            builder.RegisterCommandHandler<DisposePayload, WorkerInstanceManager>();
        }

        public void Start(string? startupType)
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
                             .AddSingleton<ICommandHandler<RunProgramPayload, ProgramFinishedPayload>>(this)
                             .AddSingleton<ICommandHandler<CancelPayload>>(this)
                             .AddSingleton<ICommandHandler<DisposePayload>>(this)
                             .AddSingleton(_serializer)
                             .AddSingleton(configuration)
                             .AddSingleton<IMessagingProvider, WorkerThreadMessagingProvider>()
                             .AddSingleton<IMessagingService, MessagingService>()
                             .AddSingleton(new WorkerIdAccessor(Id))
                             .AddSingleton(new WorkerCancellationTokenAccessor(_cancellationTokenSource.Token))
                             .AddScoped<HttpClient>();

            Type[] corePayloads = new[] { typeof(RunProgramPayload), typeof(CancelPayload), typeof(DisposePayload) };

            foreach (MessageMapping mapping in configuration.MessageMappings.Values)
            {
                if (corePayloads.Contains(mapping.PayloadType))
                    continue;

                serviceCollection.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);
            }

            _serviceProvider = serviceCollection.BuildServiceProvider();

            //Confirm initialization
            IMessagingService messaging = _serviceProvider.GetRequiredService<IMessagingService>();

            messaging.PostMessage(new Message<InitializedPayload>
            {
                Id = 0,
                WorkerId = Id,
                TargetWorkerId = null,
                Type = MessageTypes.FromWorker | MessageTypes.Result,
                Payload = new InitializedPayload()
            });
        }

        #region Core Handlers
        async ValueTask<ProgramFinishedPayload> ICommandHandler<RunProgramPayload, ProgramFinishedPayload>.HandleAsync(RunProgramPayload request, CancellationToken cancellationToken)
        {
            _ = _serviceProvider ?? throw new InvalidOperationException("Worker not initialized.");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IWorkerProgram workerProgram = scope.ServiceProvider.GetRequiredService<IWorkerProgram>();
                int exitCode = await workerProgram.Main(cancellationToken);

                return new ProgramFinishedPayload
                {
                    ExitCode = exitCode
                };
            }
        }

        ValueTask<CommandFinished> ICommandHandler<CancelPayload, CommandFinished>.HandleAsync(CancelPayload request, CancellationToken cancellationToken)
        {
            if (request.Delay == TimeSpan.Zero)
                _cancellationTokenSource.Cancel();
            else
                _cancellationTokenSource.CancelAfter(request.Delay);

            return CommandFinished.Task;
        }

        ValueTask<CommandFinished> ICommandHandler<DisposePayload, CommandFinished>.HandleAsync(DisposePayload request, CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            _serviceProvider?.Dispose();
            _cancellationTokenSource.Dispose();

            return CommandFinished.Task;
        }
        #endregion
    }
}
