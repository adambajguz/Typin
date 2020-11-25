namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System.Threading.Tasks;

    public interface IWorkerFactory
    {
        Task<IWorker> CreateAsync<T>()
            where T : class, IWebWorkerEntryPoint;
    }
}
