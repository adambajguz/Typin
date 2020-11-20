namespace TypinExamples.Core.Handlers.Notifications
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TypinExamples.Core.Handlers.Core;

    public class PingNotification : ICoreNotification
    {

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
