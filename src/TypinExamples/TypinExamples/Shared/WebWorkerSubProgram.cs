namespace TypinExamples.Shared
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BlazorWorker.WorkerCore;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebWorkerSubProgram : IWebWorkerEntryPoint
    {
        private readonly IWorkerMessageService _messageService;
        private readonly HttpClient _httpClient;

        //public WebWorkerSubProgram(IWorkerMessageService messageService, HttpClient httpClient)
        //{
        //    _messageService = messageService;
        //    _httpClient = httpClient;
        //}

        public Task<int> Main()
        {
            return Task.FromResult(new Random().Next());
        }
    }
}
