namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers;

    internal sealed class MainMessagingService : IMessagingService
    {
        private readonly IdProvider _idProvider = new();
        private readonly Dictionary<ulong, TaskCompletionSource<object>> messageRegister = new();

        private readonly ISerializer _serializer;
        private readonly IWorkerFactory _workerFactory;
        private readonly CancellationToken _cancellationToken;
        private readonly IMessagingProvider _messagingProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MainMessagingService(ISerializer serializer,
                                    IWorkerFactory workerFactory,
                                    IMessagingProvider messagingProvider,
                                    IServiceScopeFactory serviceScopeFactory)
        {
            _serializer = serializer;
            _workerFactory = workerFactory;
            _cancellationToken = CancellationToken.None;
            _messagingProvider = messagingProvider;
            _serviceScopeFactory = serviceScopeFactory;

            _messagingProvider.Callbacks += OnMessage;
        }

        public async Task PostAsync(ulong? workerId, IMessage message)
        {
            string? serialized = _serializer.Serialize(message);

            await _messagingProvider.PostAsync(workerId, serialized);
        }

        public MessageIdReservation ReserveId()
        {
            var callId = _idProvider.Next();
            var taskCompletionSource = new TaskCompletionSource<object>();
            messageRegister.Add(callId, taskCompletionSource);

            return new MessageIdReservation(callId, taskCompletionSource.Task);
        }

        public async void OnMessage(object? sender, string rawMessage)
        {
            IMessage message = _serializer.Deserialize<IMessage>(rawMessage);

            if (message.TargetWorkerId != null)
                throw new InvalidOperationException($"Message '{message}' is not for this worker.");

            _ = _serviceScopeFactory ?? throw new InvalidOperationException("Worker not initialized.");

            if (message.Type.HasFlags(MessageTypes.Result))
            {
                if (!messageRegister.TryGetValue(message.Id, out TaskCompletionSource<object>? taskCompletionSource))
                    throw new InvalidOperationException($"Invalid message with call id {message.Id} from {message.TargetWorkerId}.");

                taskCompletionSource!.SetResult(message);
                messageRegister.Remove(message.Id);

                if (message.Exception is not null)
                    taskCompletionSource.SetException(message.Exception);
            }
            else if (message.WorkerId is not null && message.Type.HasFlags(MessageTypes.Call))
            {
                IWorker worker = _workerFactory.GetWorkerOrDefault((ulong)message.WorkerId) ?? throw new InvalidOperationException($"Unknown worker {message.WorkerId}");

                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    Type messageType = message.GetType();
                    MessageMapping mappings = MainConfiguration.MessageMappings[messageType];

                    //Type wrapperType = typeof(MessageHandlerWrapper<,>).MakeGenericType(mappings.PayloadType, mappings.ResultPayloadType);
                    object service = scope.ServiceProvider.GetRequiredService(mappings.HandlerWrapperType);

                    if (message.Type.HasFlags(MessageTypes.Command) && service is ICommandHandlerWrapper command)
                    {
                        IMessage result = await command.Handle(message, worker, _cancellationToken);

                        await PostAsync(result.TargetWorkerId, result);
                    }
                    else if (message.Type.HasFlags(MessageTypes.Notification) && service is INotificationHandlerWrapper notification)
                    {
                        await notification.Handle(message, worker, _cancellationToken);
                    }
                    else
                        throw new InvalidOperationException($"Unknown message handler {service.GetType()}");
                }
            }
            else if (message.Type.HasFlags(MessageTypes.Exception))
            {
                throw message.Exception ?? throw new NullReferenceException(message.ToString());
            }
            else
                throw new InvalidOperationException($"Unknown message type {message.Type}");
        }

        public async Task NotifyAsync<TPayload>(ulong? workerId, TPayload payload)
        {
            ulong callId = _idProvider.Next();

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = null,
                TargetWorkerId = workerId,
                Type = MessageTypes.FromMain | MessageTypes.CallNotification,
                Payload = payload
            };

            await PostAsync(workerId, message);
        }

        public async Task CallCommandAsync<TPayload>(ulong? workerId, TPayload payload)
        {
            await CallCommandAsync<TPayload, CommandFinished>(workerId, payload);
        }

        public async Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(ulong? workerId, TPayload payload)
        {
            (ulong callId, Task<object> task) = ReserveId();

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = null,
                TargetWorkerId = workerId,
                Type = MessageTypes.FromMain | MessageTypes.CallCommand,
                Payload = payload
            };

            await PostAsync(workerId, message);

            if (await task is not Message<TResultPayload> returnMessage)
                throw new InvalidOperationException("Invalid message.");

            if (returnMessage.Exception is not null)
                throw new AggregateException($"Worker exception: {returnMessage.Exception.Message}", returnMessage.Exception);

            return returnMessage.Payload!;
        }

        public void Dispose()
        {
            _messagingProvider.Callbacks -= OnMessage;
        }
    }
}
