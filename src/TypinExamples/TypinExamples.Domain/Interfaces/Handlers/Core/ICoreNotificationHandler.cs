namespace TypinExamples.Domain.Interfaces.Handlers.Core
{
    public interface ICoreNotificationHandler<in TNotification>
        where TNotification : ICoreNotification
    {

    }
}
