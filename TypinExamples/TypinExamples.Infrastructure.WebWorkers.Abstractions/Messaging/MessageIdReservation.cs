namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public struct MessageIdReservation : IEquatable<MessageIdReservation>
    {
        public ulong Id { get; }
        public Task<object> Task { get; }

        public MessageIdReservation(ulong id, Task<object> task)
        {
            Id = id;
            Task = task;
        }

        public void Deconstruct(out ulong id, out Task<object> task)
        {
            id = Id;
            task = Task;
        }

        public static implicit operator (ulong Id, Task<object> Task)(MessageIdReservation value)
        {
            return (value.Id, value.Task);
        }

        public static implicit operator MessageIdReservation((ulong Id, Task<object> Task) value)
        {
            return new MessageIdReservation(value.Id, value.Task);
        }

        public override bool Equals(object? obj)
        {
            return obj is MessageIdReservation reservation && Equals(reservation);
        }

        public bool Equals(MessageIdReservation other)
        {
            return Id == other.Id &&
                   EqualityComparer<Task<object>>.Default.Equals(Task, other.Task);
        }

        public static bool operator ==(MessageIdReservation left, MessageIdReservation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MessageIdReservation left, MessageIdReservation right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Task);
        }

        public override string? ToString()
        {
            return Id.ToString();
        }
    }
}
