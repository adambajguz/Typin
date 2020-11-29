namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    internal interface IMessageHandlerWrapper
    {
        Task<IMessage> Handle(IMessage message, CancellationToken cancellationToken);
    }

    internal interface INoResultMessageHandlerWrapper
    {
        Task Handle(IMessage message, CancellationToken cancellationToken);
    }
}
