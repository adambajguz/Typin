namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IWorkerFactory : IAsyncDisposable
    {
        IWorker? GetWorkerOrDefault(ulong id);
        Task<bool> TryDisposeWorker(ulong id);

        Task<IWorker> CreateAsync<TStartup>()
            where TStartup : class, IWorkerStartup, new();
    }
}
