namespace BlazorWorker.Core.CoreInstanceService
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class WorkerInstanceInitializeException : WorkerException
    {
        public WorkerInstanceInitializeException(string message, string fullMessage)
            : base($"Error when initializing instance: {message}", fullMessage)
        {
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
