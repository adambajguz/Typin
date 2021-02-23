namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IWorkerFactory
    {
        Task<IWorker> CreateAsync<TStartup>(Action<WorkerCreationConfiguration>? creationConfiguration = null,
                                            Action<ulong>? onInitStarted = null,
                                            Action<ulong>? onCreated = null)
            where TStartup : class, IWorkerStartup, new();
    }
}
