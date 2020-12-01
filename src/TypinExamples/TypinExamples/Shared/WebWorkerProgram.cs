namespace TypinExamples.Shared
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public class WebWorkerProgram : IWorkerProgram
    {
        private readonly HttpClient _httpClient;
        private readonly IMessagingService _messaging;

        public WebWorkerProgram(HttpClient httpClient, IMessagingService messaging)
        {
            _httpClient = httpClient;
            _messaging = messaging;
        }

        public async Task<int> Main(CancellationToken cancellationToken)
        {
            try
            {
                await _messaging.CallCommandAsync(null, new TestCommand { Value = 999888999 });
                Console.WriteLine("abc989xyz");
                await _messaging.NotifyAsync(null, new TestNotification { Value = 555444555 });
                await _messaging.NotifyAsync(null, new TestNotification { Value = 444444555 });
                await _messaging.NotifyAsync(null, new TestNotification { Value = 222444555 });
                await _messaging.NotifyAsync(null, new TestNotification { Value = 111444555 });
                Console.WriteLine("111989xyz");
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                await _messaging.CallCommandAsync(null, new TestCommand { Value = 111222111 });
                Console.WriteLine("Canceled!");
            }

            return new Random().Next();
        }
    }
}
