namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    public sealed class WorkerIdAccessor
    {
        /// <summary>
        /// Worker id or null if main worker.
        /// </summary>
        public ulong? Id { get; }

        public WorkerIdAccessor()
        {
            Id = null;
        }

        public WorkerIdAccessor(ulong id)
        {
            Id = id;
        }
    }
}
