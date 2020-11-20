namespace TypinExamples.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public sealed class WorkerMessageModel
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

        [JsonProperty("fw")]
        public bool FromWorker { get; internal init; }

        [JsonProperty("cmd")]
        public string? TargetCommandType { get; internal init; }

        [JsonProperty("nfnc")]
        public string? TargetNotificationType { get; internal init; }

        [JsonProperty("data")]
        public string? Data { get; internal init; }

        [JsonProperty("args")]
        public Dictionary<string, object> Arguments { get; internal init; } = new();

        internal WorkerMessageModel()
        {

        }
    }
}
