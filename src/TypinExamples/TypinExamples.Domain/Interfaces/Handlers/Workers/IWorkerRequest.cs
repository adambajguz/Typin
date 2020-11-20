namespace TypinExamples.Domain.Interfaces.Handlers.Workers
{
    using MediatR;
    using TypinExamples.Domain.Models;

    public interface IWorkerRequest : IRequest<WorkerMessageModel>
    {

    }
}
