namespace TypinExamples.Core.Handlers.Workers
{
    public interface IWorkerNotificationHandler<in TNotification>
        where TNotification : IWorkerNotification
    {

    }
}
