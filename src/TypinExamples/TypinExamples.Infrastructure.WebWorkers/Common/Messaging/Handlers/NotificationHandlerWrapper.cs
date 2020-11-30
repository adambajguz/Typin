namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    internal class NotificationHandlerWrapper<TRequest> : INotificationHandlerWrapper
    {
        private readonly INotificationHandler<TRequest> _handler;

        public NotificationHandlerWrapper(INotificationHandler<TRequest> handler)
        {
            _handler = handler;
        }

        public async Task Handle(IMessage message, CancellationToken cancellationToken)
        {
            try
            {
                Message<TRequest>? casted = message as Message<TRequest>;
                await _handler.HandleAsync(casted.Payload, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
