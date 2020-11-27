namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IWorkerFactory : IAsyncDisposable
    {
        Task<IWorker> CreateAsync<TStartup>()
            where TStartup : class, IWorkerStartup, new();
    }
}
