namespace TypinExamples.Application.Services.Workers
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Domain.Models.Workers;

    public interface IWorkerMessageDispatcher : IAsyncDisposable
    {
        Task<WorkerResult> DispachAsync(WorkerMessage model);
    }
}