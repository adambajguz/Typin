namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;

    public sealed class WorkerError
    {
        public string? StackTrace { get; init; }
        public string? Source { get; init; }
        public string? Message { get; init; }
        public string? Text { get; init; }

        public WorkerError()
        {

        }

        public static WorkerError FromException(Exception ex)
        {
            return new WorkerError
            {
                StackTrace = ex.StackTrace,
                Source = ex.Source,
                Message = ex.Message,
                Text = ex.ToString()
            };
        }
    }
}
