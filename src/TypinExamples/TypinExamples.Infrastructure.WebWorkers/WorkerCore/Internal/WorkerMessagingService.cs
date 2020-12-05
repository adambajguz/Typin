namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal
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
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal;

    internal sealed class WorkerMessagingService : IMessagingService
    {
        private readonly IdProvider _idProvider = new();
        private readonly Dictionary<ulong, TaskCompletionSource<object>> messageRegister = new();

        private readonly ISerializer _serializer;
        private readonly IWorker _worker;
        private readonly WorkerConfiguration _configuration;
        private readonly WorkerIdAccessor _workerIdAccessor;
        private readonly CancellationToken _cancellationToken;
        private readonly IMessagingProvider _messagingProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public WorkerMessagingService(ISerializer serializer,
                                      IWorker worker,
                                      WorkerConfiguration configuration,
                                      WorkerIdAccessor workerIdAccessor,
                                      WorkerCancellationTokenAccessor cancellationTokenAccessor,
                                      IMessagingProvider messagingProvider,
                                      IServiceScopeFactory serviceScopeFactory)
        {
            _serializer = serializer;
            _worker = worker;
            _configuration = configuration;
            _workerIdAccessor = workerIdAccessor;
            _cancellationToken = cancellationTokenAccessor.Token;
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
            try
            {
                IMessage message = _serializer.Deserialize<IMessage>(rawMessage);

                if (message.TargetWorkerId != _workerIdAccessor.Id)
                    throw new InvalidOperationException($"Message '{message}' is not for this worker.");

                _ = _serviceScopeFactory ?? throw new InvalidOperationException($"Worker not initialized ({nameof(_serviceScopeFactory)} is null).");
                _ = _configuration ?? throw new InvalidOperationException($"Worker not initialized ({nameof(_configuration)} is null).");

                if (message.Type.HasFlags(MessageTypes.Result) || message.Type.HasFlags(MessageTypes.Exception))
                {
                    if (!messageRegister.TryGetValue(message.Id, out TaskCompletionSource<object>? taskCompletionSource))
                        throw new InvalidOperationException($"Invalid message with call id {message.Id} from {message.TargetWorkerId}.");

                    if (message.Error is not null)
                    {
                        taskCompletionSource.SetException(new WorkerException(message.Error));

                        Console.WriteLine(message.Error?.ToString());
                    }
                    else if (message.Type.HasFlags(MessageTypes.Exception))
                    {
                        Console.WriteLine($"Unknown error in message {message.Id}");
                    }
                    else
                        taskCompletionSource.SetResult(message);

                    messageRegister.Remove(message.Id);
                }
                else if (message.Type.HasFlags(MessageTypes.Call))
                {
                    using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                    {
                        Type messageType = message.GetType();
                        MessageMapping mappings = _configuration.MessageMappings[messageType];

                        //Type wrapperType = typeof(MessageHandlerWrapper<,>).MakeGenericType(mappings.PayloadType, mappings.ResultPayloadType);
                        object service = scope.ServiceProvider.GetRequiredService(mappings.HandlerWrapperType);

                        if (message.Type.HasFlags(MessageTypes.Command) && service is ICommandHandlerWrapper command)
                        {
                            IMessage result = await command.Handle(message, _worker, _cancellationToken);

                            await PostAsync(result.TargetWorkerId, result);
                        }
                        else if (message.Type.HasFlags(MessageTypes.Notification) && service is INotificationHandlerWrapper notification)
                            await notification.Handle(message, _worker, _cancellationToken);
                        else
                            throw new InvalidOperationException($"Unknown message handler {service.GetType()}");
                    }
                }
                else
                    Console.WriteLine($"Unknown message type {message.Type}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task NotifyAsync<TPayload>(ulong? targetWorkerId, TPayload payload)
        {
            ulong callId = _idProvider.Next();

            Message<TPayload> message = new()
            {
                Id = callId,
                WorkerId = _workerIdAccessor.Id,
                TargetWorkerId = targetWorkerId,
                Type = MessageTypes.FromWorker | MessageTypes.CallNotification,
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
                WorkerId = _workerIdAccessor.Id,
                TargetWorkerId = targetWorkerId,
                Type = MessageTypes.FromWorker | MessageTypes.CallCommand,
                Payload = payload
            };

            await PostAsync(targetWorkerId, message);

            if (await task is not Message<TResultPayload> returnMessage)
                throw new InvalidOperationException("Invalid message.");

            if (returnMessage?.Error is not null)
                throw new WorkerException(returnMessage.Error);

            return returnMessage!.Payload!;
        }

        public void Dispose()
        {
            _messagingProvider.Callbacks -= OnMessage;
        }
    }
}
