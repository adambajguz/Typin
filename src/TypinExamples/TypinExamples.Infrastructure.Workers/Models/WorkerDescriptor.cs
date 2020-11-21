namespace TypinExamples.Infrastructure.Workers.Models
{
    using BlazorWorker.Core;
    using BlazorWorker.WorkerBackgroundService;
    using TypinExamples.Infrastructure.Workers.WorkerServices;

    public class WorkerDescriptor
    {
        public IWorker Worker { get; init; } = default!;
        public IWorkerBackgroundService<WorkerService> BackgroundService { get; init; } = default!;

        public bool IsReady => !IsBusy && !IsDisposed;
        public bool IsBusy { get; internal set; }

        public bool IsDisposed => WGCLifetime <= 0;
        public int WGCLifetime { get; internal set; } = int.MaxValue;

        public override string? ToString()
        {
            return $"(Id: {Worker?.Identifier ?? -1}, WGCLifetime: {WGCLifetime}, {(IsReady ? "Ready" : "Busy or disposed")}, {(IsBusy ? "Busy" : "Idling")})";
        }
    }
}
