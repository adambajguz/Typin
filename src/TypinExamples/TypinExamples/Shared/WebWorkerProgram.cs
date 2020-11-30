namespace TypinExamples.Shared
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebWorkerProgram : IWorkerProgram
    {
        private readonly HttpClient _httpClient;

        public WebWorkerProgram(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> Main(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Canceled!");
            }

            return new Random().Next();
        }
    }
}
