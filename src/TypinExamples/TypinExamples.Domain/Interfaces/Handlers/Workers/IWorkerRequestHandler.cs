namespace TypinExamples.Domain.Interfaces.Handlers.Workers
{
    using MediatR;
    using TypinExamples.Domain.Models;

    public interface IWorkerRequestHandler<in TRequest> : IRequestHandler<TRequest, WorkerResult>
        where TRequest : IWorkerRequest
    {

    }
}
