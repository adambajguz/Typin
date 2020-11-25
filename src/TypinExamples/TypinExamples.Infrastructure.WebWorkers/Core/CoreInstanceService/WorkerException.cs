namespace TypinExamples.Infrastructure.WebWorkers.Core.CoreInstanceService
{
    using System;

    public abstract class WorkerException : Exception
    {
        public WorkerException(string message, string fullMessage) : base(message)
        {
            FullMessage = fullMessage;
        }

        public string FullMessage { get; }

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine} --> Worker full exception: {FullMessage}";
        }
    }
}
