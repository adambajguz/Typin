namespace TypinExamples.Domain.Models.Workers
{
    using System;
    using Newtonsoft.Json;

    public sealed class WorkerResult : IEquatable<WorkerResult?>
    {
        [JsonProperty("id")]
        public Guid Id { get; } = Guid.NewGuid();

        [JsonProperty("cr")]
        public DateTime CreatedOn { get; } = DateTime.UtcNow;

        [JsonProperty("mid")]
        public Guid MessageId { get; init; }

        [JsonProperty("wid")]
        public long? WorkerId { get; set; }

        [JsonProperty("fw")]
        public bool FromWorker { get; init; }

        [JsonProperty("ex")]
        public bool IsException { get; init; }

        [JsonProperty("data")]
        public string? Data { get; init; }

        public WorkerResult()
        {

        }

        public static WorkerResult CreateWorkerConfirmation(Guid messageId, long workerId)
        {
            return new WorkerResult
            {
                MessageId = messageId,
                WorkerId = workerId,
                FromWorker = true
            };
        }

        public static WorkerResult CreateWorkerException(Guid messageId, long workerId, WorkerMessage exceptionMessage)
        {
            return new WorkerResult
            {
                MessageId = messageId,
                WorkerId = workerId,
                FromWorker = true,
                IsException = true,
                Data = JsonConvert.SerializeObject(exceptionMessage)
            };
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as WorkerResult);
        }

        public bool Equals(WorkerResult? other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   CreatedOn == other.CreatedOn &&
                   WorkerId == other.WorkerId &&
                   FromWorker == other.FromWorker &&
                   Data == other.Data;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CreatedOn, WorkerId, FromWorker, Data);
        }

        public override string? ToString()
        {
            return $"{Id}: {WorkerId}";
        }
    }
}
