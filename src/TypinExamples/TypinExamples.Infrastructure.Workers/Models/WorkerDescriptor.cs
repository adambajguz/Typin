namespace TypinExamples.Workers.Models
{
    using BlazorWorker.Core;
    using BlazorWorker.WorkerBackgroundService;
    using TypinExamples.Workers.Services;

    public class WorkerDescriptor
    {
        public IWorker Worker { get; init; } = default!;
        public IWorkerBackgroundService<WorkerService> BackgroundService { get; init; } = default!;

        public bool IsInUse { get; set; }
    }
}
