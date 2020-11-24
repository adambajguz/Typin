namespace TypinExamples.Domain.Interfaces.Handlers.Workers
{
    public interface IWorkerNotificationHandler<in TNotification>
        where TNotification : IWorkerNotification
    {

    }
}
