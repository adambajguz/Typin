namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IWorker : IWorkerMessageService, IAsyncDisposable
    {
        ulong Id { get; }

        bool IsInitialized { get; }

        Task InitAsync(string initEndpoint);

        Task<int> RunAsync();
    }
}
