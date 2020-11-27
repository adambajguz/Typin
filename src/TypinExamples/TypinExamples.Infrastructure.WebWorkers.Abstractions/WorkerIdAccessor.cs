namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    public sealed class WorkerIdAccessor
    {
        public ulong Id { get; }

        public WorkerIdAccessor(ulong id)
        {
            Id = id;
        }
    }
}
