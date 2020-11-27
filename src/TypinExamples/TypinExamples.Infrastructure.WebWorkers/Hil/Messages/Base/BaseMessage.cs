namespace TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base
{
    using System;

    public abstract class BaseMessage
    {
        public ulong CallId { get; init; }
        public Exception? Exception { get; init; }
    }
}