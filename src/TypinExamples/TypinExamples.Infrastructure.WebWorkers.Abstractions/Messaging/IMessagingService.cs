namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System;

    public interface IMessagingService : IDisposable
    {
        void PostMessage(IMessage message);
    }
}