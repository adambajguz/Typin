namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    internal class NotificationHandlerWrapper<TRequest> : INotificationHandlerWrapper
    {
        private readonly INotificationHandler<TRequest> _handler;

        public NotificationHandlerWrapper(INotificationHandler<TRequest> handler)
        {
            _handler = handler;
        }

        public async Task Handle(IMessage message, IWorker worker, CancellationToken cancellationToken)
        {

            bool isNotification = message.Type.HasFlags(MessageTypes.Notification);

            try
            {
                if (!isNotification)
                    throw new InvalidOperationException("Cannot handle message that is not a notification call.");

                Message<TRequest>? casted = message as Message<TRequest>;
                await _handler.HandleAsync(casted.Payload, worker, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
