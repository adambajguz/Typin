namespace BlazorWorker.WorkerBackgroundService
{
    using System;

    public interface IEventWrapper
    {
        long InstanceId { get; }
        long EventHandleId { get; }
        Action Unregister { get; set; }
    }

}
