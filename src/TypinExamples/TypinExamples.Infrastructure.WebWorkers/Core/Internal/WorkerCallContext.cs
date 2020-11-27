namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    public record WorkerCallContext
    {
        public ulong WorkerId { get; init; }
        public ulong CallId { get; init; }
    }
}
