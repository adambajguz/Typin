namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;

    public interface ICommandHandler<in TRequest> : ICommandHandler<TRequest, CommandFinished>
        where TRequest : ICommand<CommandFinished>
    {

    }

    public interface ICommandHandler<in TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        ValueTask<TResponse> HandleAsync(TRequest request, IWorker worker, CancellationToken cancellationToken);
    }
}
