namespace TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base
{
    using System;

    public abstract class BaseMessage
    {
        public Exception? Exception { get; init; }
    }
}