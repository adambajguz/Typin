namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMessageHandler<in TRequest>
    {
        ValueTask HandleAsync(TRequest request, CancellationToken cancellationToken);
    }

    public interface IMessageHandler<in TRequest, TResponse>
    {
        ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
