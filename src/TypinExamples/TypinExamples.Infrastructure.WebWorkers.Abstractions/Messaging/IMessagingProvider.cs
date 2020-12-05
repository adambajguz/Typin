namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;
    using System.Threading.Tasks;

    public interface IMessagingProvider : IDisposable
    {
        event EventHandler<string> Callbacks;

        void OnMessage(string rawMessage);
        Task PostAsync(ulong? targetWorkerId, string rawMessage);
    }
}