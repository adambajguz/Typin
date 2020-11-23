namespace BlazorWorker.Core.CoreInstanceService
{
    using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
