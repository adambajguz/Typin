namespace TypinExamples.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Domain.Models;

    public interface ICoreTaskDispatcher
    {
        Task DispatchAsync(WorkerMessageModel model);
    }
}