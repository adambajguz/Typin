namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System.Threading.Tasks;

    public interface IWorkerFactory
    {
        Task<IWorker> CreateAsync<TStartup>()
            where TStartup : class, IWorkerStartup, new();
    }
}
