namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IWorker : IAsyncDisposable
    {
        ulong Id { get; }

        bool IsInitialized { get; }
        bool IsDisposed { get; }

        Task<int> RunAsync();
        Task CancelAsync();
        Task<TResponse> CallAsync<TRequest, TResponse>(TRequest data);
    }
}
