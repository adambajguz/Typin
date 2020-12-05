namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging
{
    using System;
    using System.Collections.Generic;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers;

    public sealed class Message<TRequest> : IMessage, IEquatable<Message<TRequest>?>
    {
        public ulong Id { get; init; }
        public ulong? WorkerId { get; init; } = null;
        public ulong? TargetWorkerId { get; init; } = null;
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;
        public MessageTypes Type { get; init; }

        public WorkerError? Error { get; init; }
        public TRequest? Payload { get; init; } = default!;

        public Message()
        {

        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as Message<TRequest>);
        }

        public bool Equals(Message<TRequest>? other)
        {
            return other != null &&
                   Id == other.Id &&
                   TargetWorkerId == other.TargetWorkerId &&
                   Timestamp.Equals(other.Timestamp) &&
                   Type == other.Type &&
                   EqualityComparer<TRequest?>.Default.Equals(Payload, other.Payload);
        }

        public static bool operator ==(Message<TRequest>? left, Message<TRequest>? right)
        {
            return EqualityComparer<Message<TRequest>>.Default.Equals(left, right);
        }

        public static bool operator !=(Message<TRequest>? left, Message<TRequest>? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, TargetWorkerId, Timestamp, Type, Error, Payload);
        }

        public override string? ToString()
        {
            return $"({nameof(Id)}: {Id}, {nameof(TargetWorkerId)}: {TargetWorkerId}, {nameof(Timestamp)}: {Timestamp}, {nameof(Type)}: {Type})";
        }
    }
}
