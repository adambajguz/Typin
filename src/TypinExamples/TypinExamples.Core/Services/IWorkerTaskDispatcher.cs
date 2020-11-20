namespace TypinExamples.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Core.Models;

    public interface IWorkerTaskDispatcher : IAsyncDisposable
    {
        Task<WorkerMessageModel> RunAsync(WorkerMessageModel dataModel);
        Task<bool> SendMessageAsync(Guid id, WorkerMessageModel model);
    }
}