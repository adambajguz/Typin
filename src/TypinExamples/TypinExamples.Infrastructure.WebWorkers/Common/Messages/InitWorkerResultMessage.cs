namespace TypinExamples.Infrastructure.WebWorkers.Common
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class InitWorkerResultMessage : IMessage
    {
        public ulong WorkerId { get; init; }
        public ulong CallId { get; init; }
        public Exception? Exception { get; init; }
    }
}
