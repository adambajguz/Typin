namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface INotificationHandler<in TRequest>
    {
        ValueTask HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
