namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    public interface IWorker : IWorkerMessageService, IAsyncDisposable
    {
        bool IsInitialized { get; }

        long Identifier { get; }

        Task InitAsync(string initEndpoint);

        Task<int> RunAsync();
    }
}
