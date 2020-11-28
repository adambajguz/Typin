namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public sealed class Message<TRequest> : IMessage<TRequest>
    {
        public ulong Id { get; init; }
        public ulong WorkerId { get; init; }
        public bool FromWorker { get; init; }
        public bool IsResult { get; init; }

        public Exception? Exception { get; init; }
        public TRequest? Payload { get; init; } = default!;
    }
}
