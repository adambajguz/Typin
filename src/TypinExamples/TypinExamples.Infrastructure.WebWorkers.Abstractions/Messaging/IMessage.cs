namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;

    public interface IMessage
    {
        ulong Id { get; init; }
        ulong? WorkerId { get; init; }
        ulong? TargetWorkerId { get; init; }
        public DateTimeOffset Timestamp { get; }
        MessageTypes Type { get; init; }

        WorkerError? Error { get; init; }
    }
}