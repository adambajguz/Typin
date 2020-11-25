namespace BlazorWorker.WorkerBackgroundService
{
    using System.Threading.Tasks;
    using BlazorWorker.Core;

    public interface IWorkerBackgroundServiceFactory
    {
        Task<IWorker> CreateWebworkerAsync();
    }
}
