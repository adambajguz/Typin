namespace TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMessageHandler<in TRequest, TResponse>
        where TRequest : IMessage<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
