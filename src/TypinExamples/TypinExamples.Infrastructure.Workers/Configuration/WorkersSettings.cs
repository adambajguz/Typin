namespace TypinExamples.Infrastructure.Workers.Configuration
{
    public sealed class WorkersSettings
    {
        /// <summary>
        /// Whether worker garbage collector is enabled.
        /// </summary>
        public bool WGCEnabled { get; init; } = true;

        /// <summary>
        /// Worker garbage collector interval. Cannot be lower than 100 ms.
        /// </summary>
        public int WGCInterval { get; init; } = 2000;

        /// <summary>
        /// Number of worker garbage collector passes before removing a worker from pool. Cannot be lower than 1.
        /// </summary>
        public int WorkerWGCLifetime { get; init; } = 5;
    }
}
