namespace TypinExamples.Workers.Services
{
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore;
    using Newtonsoft.Json;
    using TypinExamples.Core.Services;
    using TypinExamples.Domain.Models;

    public class CoreTaskDispatcher : ICoreTaskDispatcher
    {
        private readonly IWorkerMessageService _workerMessageService;

        public CoreTaskDispatcher(IWorkerMessageService workerMessageService)
        {
            _workerMessageService = workerMessageService;
        }

        public async Task DispatchAsync(WorkerMessageModel model)
        {
            string serializedModel = JsonConvert.SerializeObject(model);
            await _workerMessageService.PostMessageAsync(serializedModel);
        }
    }
}
