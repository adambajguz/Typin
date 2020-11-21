namespace TypinExamples.Core.Handlers.Workers.Notifications
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;

    public class WorkerPingNotification : IWorkerNotification
    {
        public long? WorkerId { get; set; }
    }

    public class WorkerPong1 : IWorkerNotificationHandler<WorkerPingNotification>
    {
        public Task Handle(WorkerPingNotification notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 1");
            return Task.CompletedTask;
        }
    }

    public class WorkerPong2 : IWorkerNotificationHandler<WorkerPingNotification>
    {
        public Task Handle(WorkerPingNotification notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Pong 2");
            return Task.CompletedTask;
        }
    }
}
