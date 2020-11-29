namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging;

    public class WorkerInstanceManager :
        IMessageHandler<Dispose.Payload, Dispose.ResultPayload>,
        IMessageHandler<RunProgram.Payload, RunProgram.ResultPayload>,
        IMessageHandler<Cancel.Payload, Cancel.ResultPayload>
    {
        private readonly IdProvider _idProvider = new();

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly ISerializer _serializer;

        private ServiceProvider? _serviceProvider;
        private WorkerConfiguration? _configuration;

        private readonly Dictionary<ulong, TaskCompletionSource<object>> messageRegister = new();

        public ulong Id { get; }

        public WorkerInstanceManager(ulong id, ISerializer? serializer = null)
        {
            Id = id;

            _serializer = serializer ?? new DefaultSerializer();
            MessageService.Message += OnMessage;
        }

        #region Messaging
        public void PostMessage<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            string? serialized = _serializer.Serialize(message);

            MessageService.PostMessage(serialized);
        }

        private async Task<TResultPayload?> PostMessageAsync<TPayload, TResultPayload>(TPayload payload)
        {
            var callId = _idProvider.Next();

            var taskCompletionSource = new TaskCompletionSource<object>();
            messageRegister.Add(callId, taskCompletionSource);

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = Id,
                Payload = payload
            };

            PostMessage(message);

            if (await taskCompletionSource.Task is not Message<TResultPayload> returnMessage)
                throw new InvalidOperationException("Invalid message.");

            if (returnMessage.Exception is not null)
                throw new AggregateException($"Worker exception: {returnMessage.Exception.Message}", returnMessage.Exception);

            return returnMessage.Payload;
        }

        private async void OnMessage(object? sender, string rawMessage)
        {
            IMessage message = _serializer.Deserialize<IMessage>(rawMessage);

            _ = _serviceProvider ?? throw new InvalidOperationException("Worker not initialized.");
            _ = _configuration ?? throw new InvalidOperationException("Worker not initialized.");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                Type messageType = message.GetType();
                MessageMapping mappings = _configuration.MessageMappings[messageType];

                //Type wrapperType = typeof(MessageHandlerWrapper<,>).MakeGenericType(mappings.PayloadType, mappings.ResultPayloadType);
                object service = scope.ServiceProvider.GetRequiredService(mappings.HandlerWrapperType);

                if (service is IMessageHandlerWrapper wrapper)
                {
                    IMessage result = await wrapper.Handle(message, _cancellationTokenSource.Token);

                    PostMessage(result);
                }
                else if(service is INoResultMessageHandlerWrapper noResultWrapper)
                {
                    await noResultWrapper.Handle(message, _cancellationTokenSource.Token);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown message handler {service.GetType()}");
                }
            }
        }
        #endregion

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
            _configuration = configuration;

            ServiceCollection serviceCollection = new ServiceCollection();
            startup.ConfigureServices(serviceCollection);

            serviceCollection.AddTransient(typeof(IWorkerProgram), configuration.ProgramType)
                             .AddTransient(typeof(MessageHandlerWrapper<,>))
                             .AddTransient<IWorkerMessageService, InjectableMessageService>()
                             .AddSingleton<IMessageHandler<Dispose.Payload, Dispose.ResultPayload>>(this)
                             .AddSingleton<IMessageHandler<RunProgram.Payload, RunProgram.ResultPayload>>(this)
                             .AddSingleton<IMessageHandler<Cancel.Payload, Cancel.ResultPayload>>(this)
                             .AddSingleton(configuration)
                             .AddSingleton(new WorkerIdAccessor(Id))
                             .AddScoped<HttpClient>();

            foreach (MessageMapping mapping in configuration.MessageMappings.Values)
            {
                serviceCollection.TryAddTransient(mapping.HandlerInterfaceType, mapping.HandlerType);
            }

            _serviceProvider = serviceCollection.BuildServiceProvider();

            PostMessage(new Message<Init.ResultPayload>
            {
                Id = 0,
                WorkerId = Id,
                FromWorker = true,
                IsResult = true,
                Payload = new Init.ResultPayload()
            });
        }

        public ValueTask<Dispose.ResultPayload> HandleAsync(Dispose.Payload request, CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            _serviceProvider?.Dispose();
            _cancellationTokenSource.Dispose();
            MessageService.Message -= OnMessage;

            return ValueTask.FromResult(new Dispose.ResultPayload
            {
                IsSuccess = true,
            });
        }

        public async ValueTask<RunProgram.ResultPayload> HandleAsync(RunProgram.Payload request, CancellationToken cancellationToken)
        {
            _ = _serviceProvider ?? throw new InvalidOperationException("Worker not initialized.");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IWorkerProgram workerProgram = scope.ServiceProvider.GetRequiredService<IWorkerProgram>();
                int exitCode = await workerProgram.Main(cancellationToken);
                Console.WriteLine("44444444444444444   " + exitCode.ToString());

                return new RunProgram.ResultPayload
                {
                    ExitCode = exitCode
                };
            }
        }

        public ValueTask<Cancel.ResultPayload> HandleAsync(Cancel.Payload request, CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            return ValueTask.FromResult(new Cancel.ResultPayload());
        }
    }
}
