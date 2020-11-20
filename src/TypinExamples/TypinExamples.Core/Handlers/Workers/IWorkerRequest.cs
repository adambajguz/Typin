namespace TypinExamples.Core.Handlers.Workers
{
    using MediatR;
    using TypinExamples.Core.Models;

    public interface IWorkerRequest : IRequest<WorkerMessageModel>
    {

    }
}
