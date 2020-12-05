namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;
    using System.Threading.Tasks;

    public interface IMessagingService : IDisposable
    {
        Task PostAsync(ulong? workerId, IMessage message);
        MessageIdReservation ReserveId(ulong? targetWorkerId);

        Task NotifyAsync<TPayload>(ulong? targetWorkerId, TPayload payload);
        Task CallCommandAsync<TPayload>(ulong? targetWorkerId, TPayload payload);
        Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(ulong? targetWorkerId, TPayload payload);

        void CleanMessageRegistry(ulong workerId);
    }
}