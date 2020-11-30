namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public sealed class Message<TRequest> : IMessage
    {
        public ulong Id { get; init; }
        public ulong WorkerId { get; init; }
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;
        public MessageTypes Type { get; init; }

        public Exception? Exception { get; init; }
        public TRequest? Payload { get; init; } = default!;
    }
}
