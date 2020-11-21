namespace TypinExamples.Application.Handlers.Notifications.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Domain.Events;
    using TypinExamples.Domain.Interfaces.Handlers.Core;

    public class ExceptionFromWorkerNotificationHandler : ICoreNotificationHandler<ExceptionFromWorkerNotification>
    {
        public Task Handle(ExceptionFromWorkerNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
