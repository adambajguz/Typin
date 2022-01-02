namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public interface IWorker : IAsyncDisposable
    {
        ulong Id { get; }

        bool IsInitialized { get; }
        bool IsCancelled { get; }
        bool IsDisposed { get; }

        Task<int> RunAsync();
        Task CancelAsync();
        Task CancelAsync(TimeSpan delay);

        Task NotifyAsync<TNotification>(TNotification payload)
            where TNotification : INotification;

        Task CallCommandAsync<TCommand>(TCommand payload)
            where TCommand : ICommand;

        Task<TResult> CallCommandAsync<TCommand, TResult>(TCommand payload)
            where TCommand : ICommand<TResult>;
    }
}
