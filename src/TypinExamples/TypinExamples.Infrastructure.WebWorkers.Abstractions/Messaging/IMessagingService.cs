namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;
    using System.Threading.Tasks;

    public interface IMessagingService : IDisposable
    {
        Task PostAsync(ulong? workerId, IMessage message);
        MessageIdReservation ReserveId();

        Task NotifyAsync<TPayload>(ulong? workerId, TPayload? payload);
        Task CallCommandAsync<TPayload>(ulong? workerId, TPayload payload);
        Task<TResultPayload> CallCommandAsync<TPayload, TResultPayload>(ulong? workerId, TPayload payload);
    }
}