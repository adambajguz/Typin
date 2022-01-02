namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IWorkerManager : IAsyncDisposable
    {
        void AddWorker(IWorker worker);
        bool RemoveWorker(ulong id);
        IWorker? GetWorkerOrDefault(ulong id);
        Task<bool> TryDisposeWorker(ulong id);
    }
}
