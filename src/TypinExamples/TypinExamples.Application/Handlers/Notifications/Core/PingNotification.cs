namespace TypinExamples.Application.Handlers.Notifications.Core
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Domain.Interfaces.Handlers.Core;

    public class PingNotification : ICoreNotification
    {
        public long? WorkerId { get; set; }
    }

    public class Pong1 : ICoreNotificationHandler<PingNotification>
    {
        public Task Handle(PingNotification notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 1");
            return Task.CompletedTask;
        }
    }

    public class Pong2 : ICoreNotificationHandler<PingNotification>
    {
        public Task Handle(PingNotification notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 2");
            return Task.CompletedTask;
        }
    }
}
