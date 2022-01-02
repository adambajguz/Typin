namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Extensions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    internal class NotificationHandlerWrapper<TNotification> : INotificationHandlerWrapper
        where TNotification : INotification
    {
        private readonly INotificationHandler<TNotification> _handler;

        public NotificationHandlerWrapper(INotificationHandler<TNotification> handler)
        {
            _handler = handler;
        }

        public async Task Handle(IMessage message, IWorker worker, CancellationToken cancellationToken)
        {

            bool isNotification = message.Type.HasFlags(MessageTypes.Notification);

            try
            {
                if (!isNotification)
                {
                    throw new InvalidOperationException("Cannot handle message that is not a notification call.");
                }

                Message<TNotification> casted = message as Message<TNotification> ?? throw new NullReferenceException("Invalid notification message type.");
                await _handler.HandleAsync(casted.Payload!, worker, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
