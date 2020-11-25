namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public interface IWorkerBackgroundServiceFactory
    {
        Task<IWorker> CreateWebworkerAsync();
    }
}
