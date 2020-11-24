namespace BlazorWorker.Core.CoreInstanceService
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class WorkerInstanceDisposeException : WorkerException
    {
        public WorkerInstanceDisposeException(string message, string fullMessage)
            : base($"Error when disposing instance: {message}", fullMessage)
        {
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
