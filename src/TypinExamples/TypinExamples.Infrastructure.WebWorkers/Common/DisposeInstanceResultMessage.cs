namespace TypinExamples.Infrastructure.WebWorkers.Common
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class DisposeInstanceResultMessage : IMessage
    {
        public ulong WorkerId { get; init; }
        public ulong CallId { get; init; }
        public Exception? Exception { get; init; }

        public bool IsSuccess { get; init; }
    }
}
