namespace TypinExamples.Core.Handlers.Workers
{
    using MediatR;
    using TypinExamples.Core.Models;

    public interface IWorkerRequestHandler<in TRequest> : IRequestHandler<TRequest, WorkerMessageModel>
        where TRequest : IRequest<WorkerMessageModel>
    {

    }
}
