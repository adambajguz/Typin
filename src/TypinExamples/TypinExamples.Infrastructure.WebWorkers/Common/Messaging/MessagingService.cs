namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging
{
    using System;
    using System.Threading;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers;

    public sealed class MessagingService : IMessagingService
    {
        private readonly ISerializer _serializer;
        private readonly WorkerConfiguration _configuration;
        private readonly WorkerIdAccessor _workerIdAccessor;
        private readonly CancellationToken _cancellationToken;
        private readonly IMessagingProvider _messagingProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MessagingService(ISerializer serializer,
                                WorkerConfiguration configuration,
                                WorkerIdAccessor workerIdAccessor,
                                WorkerCancellationTokenAccessor cancellationTokenAccessor,
                                IMessagingProvider messagingProvider,
                                IServiceScopeFactory serviceScopeFactory)
        {
            _serializer = serializer;
            _configuration = configuration;
            _workerIdAccessor = workerIdAccessor;
            _cancellationToken = cancellationTokenAccessor.Token;
            _messagingProvider = messagingProvider;
            _serviceScopeFactory = serviceScopeFactory;

            _messagingProvider.Callbacks += OnMessage;
        }

        public void PostMessage(IMessage message)
        {
            string? serialized = _serializer.Serialize(message);

            _messagingProvider.PostAsync(_workerIdAccessor.Id, serialized);
        }

        public async void OnMessage(object? sender, string rawMessage)
        {
            try
            {
                IMessage message = _serializer.Deserialize<IMessage>(rawMessage);

                if (message.TargetWorkerId != _workerIdAccessor.Id)
                    throw new InvalidOperationException($"Message '{message}' is not for this worker.");

                _ = _serviceScopeFactory ?? throw new InvalidOperationException("Worker not initialized.");
                _ = _configuration ?? throw new InvalidOperationException("Worker not initialized.");

                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    Type messageType = message.GetType();
                    MessageMapping mappings = _configuration.MessageMappings[messageType];

                    //Type wrapperType = typeof(MessageHandlerWrapper<,>).MakeGenericType(mappings.PayloadType, mappings.ResultPayloadType);
                    object service = scope.ServiceProvider.GetRequiredService(mappings.HandlerWrapperType);

                    if (service is ICommandHandlerWrapper command)
                    {
                        IMessage result = await command.Handle(message, _cancellationToken);

                        PostMessage(result);
                    }
                    else if (service is INotificationHandlerWrapper notification)
                        await notification.Handle(message, _cancellationToken);
                    else
                        throw new InvalidOperationException($"Unknown message handler {service.GetType()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            _messagingProvider.Callbacks -= OnMessage;
        }
    }
}
