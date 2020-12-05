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
        private readonly IWorkerManager _workerManager;
        private readonly CancellationToken _cancellationToken;
        private readonly IMessagingProvider _messagingProvider;
        private readonly IServiceProvider _serviceProvider;

        public MainMessagingService(ISerializer serializer,
                                    IWorkerManager workerManager,
                                    IMessagingProvider messagingProvider,
                                    IServiceProvider serviceProvider)
        {
            _serializer = serializer;
            _workerManager = workerManager;
            _cancellationToken = CancellationToken.None;
            _messagingProvider = messagingProvider;
            _serviceProvider = serviceProvider;

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

            if (message.Type.HasFlags(MessageTypes.Result) || message.Type.HasFlags(MessageTypes.Exception))
            {
                if (!messageRegister.TryGetValue(message.Id, out TaskCompletionSource<object>? taskCompletionSource))
                    throw new InvalidOperationException($"Invalid message with call id {message.Id} from {message.TargetWorkerId}.");

                if (message.Error is not null)
                {
                    taskCompletionSource.SetException(new WorkerException(message.Error));

                    throw new WorkerException(message.Error ?? throw new NullReferenceException(message.ToString()));
                }
                else if (message.Type.HasFlags(MessageTypes.Exception))
                {
                    throw new InvalidOperationException($"Unknown error in message {message.Id}");
                }
                else
                    taskCompletionSource.SetResult(message);

                messageRegister.Remove(message.Id);
            }
            else if (message.WorkerId is not null && message.Type.HasFlags(MessageTypes.Call))
            {
                IWorker worker = _workerManager.GetWorkerOrDefault((ulong)message.WorkerId) ?? throw new InvalidOperationException($"Unknown worker {message.WorkerId}");

                Type messageType = message.GetType();
                MessageMapping mappings = MainConfiguration.MessageMappings[messageType];

                //Type wrapperType = typeof(MessageHandlerWrapper<,>).MakeGenericType(mappings.PayloadType, mappings.ResultPayloadType);
                object service = _serviceProvider.GetRequiredService(mappings.HandlerWrapperType);

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
            else
                throw new InvalidOperationException($"Unknown message type {message.Type}");
        }

        public async Task NotifyAsync<TPayload>(ulong? targetWorkerId, TPayload payload)
        {
            ulong callId = _idProvider.Next();

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = null,
                TargetWorkerId = targetWorkerId,
                Type = MessageTypes.FromMain | MessageTypes.CallNotification,
                Payload = payload
            };

            await PostAsync(targetWorkerId, message);
        }

        public async Task CallCommandAsync<TPayload>(ulong? targetWorkerId, TPayload payload)
        {
            await CallCommandAsync<TPayload, CommandFinished>(targetWorkerId, payload);
        }

        public async Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(ulong? targetWorkerId, TPayload payload)
        {
            (ulong callId, Task<object> task) = ReserveId();

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = null,
                TargetWorkerId = targetWorkerId,
                Type = MessageTypes.FromMain | MessageTypes.CallCommand,
                Payload = payload
            };

            await PostAsync(targetWorkerId, message);

            if (await task is not Message<TResultPayload> returnMessage)
                throw new InvalidOperationException("Invalid message.");

            if (returnMessage?.Error is not null)
                throw new WorkerException(returnMessage.Error);

            return returnMessage.Payload!;
        }

        public void Dispose()
        {
            _messagingProvider.Callbacks -= OnMessage;
        }
    }
}
