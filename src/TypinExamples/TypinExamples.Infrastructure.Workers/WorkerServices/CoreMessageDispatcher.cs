namespace TypinExamples.Infrastructure.Workers.WorkerServices
{
    using System;
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore;
    using Newtonsoft.Json;
    using TypinExamples.Application.Services.Workers;
    using TypinExamples.Domain.Models.Workers;

    public class CoreMessageDispatcher : ICoreMessageDispatcher
    {
        private readonly IWorkerMessageService _workerMessageService;

        public CoreMessageDispatcher(IWorkerMessageService workerMessageService)
        {
            _workerMessageService = workerMessageService;
        }

        public async Task DispatchAsync(WorkerMessage model)
        {
            string serializedModel = JsonConvert.SerializeObject(model);
            Console.WriteLine("y");
            Console.WriteLine(serializedModel);
            await _workerMessageService.PostMessageAsync(serializedModel);
        }
    }
}
