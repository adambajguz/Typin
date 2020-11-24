namespace TypinExamples.Domain.Interfaces.Handlers.Workers
{
    using MediatR;
    using TypinExamples.Domain.Models.Workers;

    public interface IWorkerRequest : IRequest<WorkerResult>, IWorkerIdentifiable
    {

    }
}
