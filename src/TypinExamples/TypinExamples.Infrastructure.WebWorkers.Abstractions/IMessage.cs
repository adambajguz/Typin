namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;

    public interface IMessage
    {
        ulong WorkerId { get; init; }
        ulong CallId { get; init; }
        Exception? Exception { get; init; }
    }

    public interface IMessage<TResponse> : IMessage
    {
        TResponse Data { get; init; }
    }
}