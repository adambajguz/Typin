namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;

    public interface ICommandHandler<in TRequest> : ICommandHandler<TRequest, CommandFinished>
    {

    }

    public interface ICommandHandler<in TRequest, TResponse>
    {
        ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
