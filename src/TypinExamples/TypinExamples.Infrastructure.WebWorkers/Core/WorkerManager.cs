namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public sealed class WorkerManager : IWorkerManager
    {
        private readonly Dictionary<ulong, IWorker> _workers = new();

        public WorkerManager()
        {

        }

        public void AddWorker(IWorker worker)
        {
            _workers.TryAdd(worker.Id, worker);
        }

        public bool RemoveWorker(ulong id)
        {
            return _workers.Remove(id);
        }

        public IWorker? GetWorkerOrDefault(ulong id)
        {
            _workers.TryGetValue(id, out IWorker? value);

            return value;
        }

        public async Task<bool> TryDisposeWorker(ulong id)
        {
            if (!_workers.TryGetValue(id, out IWorker? value))
                return false;

            await value.DisposeAsync();

            return true;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var worker in _workers)
                await worker.Value.DisposeAsync();

            _workers.Clear();
        }
    }
}
