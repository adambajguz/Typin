namespace TypinExamples.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Domain.Models;

    public interface IWorkerMessageDispatcher : IAsyncDisposable
    {
        Task<WorkerResult> DispachAsync(WorkerMessage model);
    }
}