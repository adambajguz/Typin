namespace BlazorWorker.Core
{
    using System;
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore;

    public interface IWorker : IWorkerMessageService, IAsyncDisposable
    {
        bool IsInitialized { get; }

        long Identifier { get; }

        Task InitAsync(WorkerInitOptions initOptions);
    }
}
