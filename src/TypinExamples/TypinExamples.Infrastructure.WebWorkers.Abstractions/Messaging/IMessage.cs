namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;

    public interface IMessage
    {
        ulong Id { get; init; }
        ulong WorkerId { get; init; }
        bool FromWorker { get; init; }
        bool IsResult { get; init; }

        Exception? Exception { get; init; }
    }

    public interface IMessage<TResponse> : IMessage
    {
        TResponse? Payload { get; init; }
    }
}