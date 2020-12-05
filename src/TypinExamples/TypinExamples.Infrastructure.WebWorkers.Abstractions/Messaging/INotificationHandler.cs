namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface INotificationHandler<in TRequest>
        where TRequest : INotification
    {
        ValueTask HandleAsync(TRequest request, IWorker worker, CancellationToken cancellationToken);
    }
}
