namespace TypinExamples.Infrastructure.WebWorkers.Core.CoreInstanceService
{
    public class WorkerInstanceDisposeException : WorkerException
    {
        public WorkerInstanceDisposeException(string message, string fullMessage)
            : base($"Error when disposing instance: {message}", fullMessage)
        {
        }
    }
}
