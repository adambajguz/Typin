using System.Threading.Tasks;
using BlazorWorker.Core;

namespace BlazorWorker.WorkerBackgroundService
{
    public interface IWorkerBackgroundServiceFactory
    {
        Task<IWorker> CreateWebworkerAsync();
    }
}
