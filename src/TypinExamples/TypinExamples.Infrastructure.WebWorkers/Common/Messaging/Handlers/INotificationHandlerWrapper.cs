namespace TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    internal interface INotificationHandlerWrapper
    {
        Task Handle(IMessage message, IWorker worker, CancellationToken cancellationToken);
    }
}
