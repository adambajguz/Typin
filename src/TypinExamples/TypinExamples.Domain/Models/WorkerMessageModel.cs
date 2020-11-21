namespace TypinExamples.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public sealed class WorkerMessageModel : IEquatable<WorkerMessageModel?>
    {
        public static WorkerMessageModel Empty { get; } = new WorkerMessageModel
        {
            Id = Guid.Empty,
            CreatedOn = DateTime.MinValue
        };

        [JsonProperty("id")]
        public Guid Id { get; internal init; } = Guid.NewGuid();

        [JsonProperty("cr")]
        public DateTime CreatedOn { get; internal init; } = DateTime.UtcNow;

        [JsonProperty("wid")]
        public long? WorkerId { get; set; }

        [JsonProperty("fw")]
        public bool FromWorker { get; internal init; }

        [JsonProperty("t")]
        public string? TargetType { get; internal init; }

        [JsonProperty("itn")]
        public bool IsNotification { get; internal init; }

        [JsonProperty("data")]
        public string? Data { get; internal init; }

        [JsonProperty("args")]
        public Dictionary<string, object> Arguments { get; internal init; } = new();

        internal WorkerMessageModel()
        {

        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as WorkerMessageModel);
        }

        public bool Equals(WorkerMessageModel? other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   CreatedOn == other.CreatedOn &&
                   WorkerId == other.WorkerId &&
                   FromWorker == other.FromWorker &&
                   TargetType == other.TargetType &&
                   IsNotification == other.IsNotification &&
                   Data == other.Data &&
                   EqualityComparer<Dictionary<string, object>>.Default.Equals(Arguments, other.Arguments);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CreatedOn, WorkerId, FromWorker, TargetType, IsNotification, Data, Arguments);
        }

        public override string? ToString()
        {
            return $"{Id}: {WorkerId} -> {TargetType}";
        }
    }
}
