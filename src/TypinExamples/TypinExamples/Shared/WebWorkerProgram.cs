namespace TypinExamples.Shared
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebWorkerProgram : IWorkerProgram
    {
        private readonly IWorkerMessageService _messageService;
        private readonly HttpClient _httpClient;

        //public WebWorkerProgram(IWorkerMessageService messageService, HttpClient httpClient)
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
