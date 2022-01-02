namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;

    public sealed class WorkerException : Exception
    {
        public WorkerError Error { get; }

        public override string Message => Error.Message ?? string.Empty;

        public override string? Source
        {
            get => Error.Source;
            set { }
        }

        public override string? StackTrace => Error.StackTrace;

        public WorkerException(WorkerError error)
        {
            Error = error;
        }

        public override string ToString()
        {
            return Error.Text ?? string.Empty;
        }
    }
}
