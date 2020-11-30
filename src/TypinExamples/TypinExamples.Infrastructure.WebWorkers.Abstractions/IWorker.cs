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

        Task NotifyAsync<TPayload>(TPayload payload);
        Task CallCommandAsync<TPayload>(TPayload payload);
        Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(TPayload payload);
    }
}
