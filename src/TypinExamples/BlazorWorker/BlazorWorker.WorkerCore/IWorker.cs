using System;
using System.Threading.Tasks;
using BlazorWorker.WorkerCore;

namespace BlazorWorker.Core
{

    public interface IWorker : IWorkerMessageService, IAsyncDisposable
    {
        bool IsInitialized { get; }

        long Identifier { get; }

        Task InitAsync(WorkerInitOptions initOptions);
    }
}
