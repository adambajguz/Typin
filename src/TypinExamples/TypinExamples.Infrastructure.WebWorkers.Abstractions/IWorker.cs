namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public interface IWorker : IAsyncDisposable
    {
        ulong Id { get; }

        bool IsInitialized { get; }
        bool IsDisposed { get; }

        Task<int> RunAsync();
        Task CancelAsync();
        Task CancelAsync(TimeSpan delay);

        Task NotifyAsync<TPayload>(TPayload payload)
            where TPayload : INotification;

        Task CallCommandAsync<TPayload>(TPayload payload)
            where TPayload : ICommand;

        Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(TPayload payload)
            where TPayload : ICommand<TResultPayload>;
    }
}
