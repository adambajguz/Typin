namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class CallResultMessage<TResponse> : IMessage<TResponse>
    {
        public ulong WorkerId { get; init; }
        public ulong CallId { get; init; }
        public Exception? Exception { get; init; }

        public TResponse Data { get; init; } = default!;
    }
}
