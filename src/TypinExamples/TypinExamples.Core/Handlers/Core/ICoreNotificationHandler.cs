namespace TypinExamples.Core.Handlers.Core
{
    public interface ICoreNotificationHandler<in TNotification>
        where TNotification : ICoreNotification
    {

    }
}
