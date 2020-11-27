namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMessageHandler<in TRequest, TResponse>
        where TRequest : IMessage<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
