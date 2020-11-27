namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class CallMessage<TRequest> : IMessage<TRequest>
    {
        public ulong WorkerId { get; init; }
        public ulong CallId { get; init; }
        public Exception? Exception { get; init; }

        public string? ProgramClass { get; init; }
        public TRequest Data { get; init; } = default!;
    }
}
