﻿namespace TypinExamples.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Domain.Models;

    public interface IWorkerTaskDispatcher : IAsyncDisposable
    {
        Task<WorkerMessageModel> DispachAsync(WorkerMessageModel model);
    }
}