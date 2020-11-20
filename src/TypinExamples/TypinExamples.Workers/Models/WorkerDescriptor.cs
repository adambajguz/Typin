namespace TypinExamples.Workers.Models
{
    using TypinExamples.Workers.Services;
    using BlazorWorker.Core;
    using BlazorWorker.WorkerBackgroundService;

    public class WorkerDescriptor
    {
        public IWorker Worker { get; init; } = default!;
        public IWorkerBackgroundService<WorkerService> BackgroundService { get; init; } = default!;

        public bool IsInUse { get; set; }
    }
}
