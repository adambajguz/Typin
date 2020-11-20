namespace TypinExamples.Core.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public sealed class WorkerMessageModel
    {
        [JsonProperty("id")]
        public Guid Id { get; init; } = Guid.NewGuid();

        [JsonProperty("cr")]
        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        [JsonProperty("fw")]
        public bool FromWorker { get; init; }

        [JsonProperty("cmd")]
        public string? Command { get; init; }

        [JsonProperty("nfnc")]
        public string? Notification { get; init; }

        [JsonProperty("args")]
        public Dictionary<string, object>? Arguments { get; init; }

        [JsonProperty("rd")]
        public byte[]? RawData { get; init; }
    }
}
