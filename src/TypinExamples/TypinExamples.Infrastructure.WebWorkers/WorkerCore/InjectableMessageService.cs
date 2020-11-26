namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class InjectableMessageService : IWorkerMessageService, IDisposable
    {
        public InjectableMessageService()
        {
            MessageService.Message += OnIncomingMessage;
        }

        private void OnIncomingMessage(object sender, string rawMessage)
        {
            IncomingMessage?.Invoke(sender, rawMessage);
        }

        public event EventHandler<string> IncomingMessage;

        public void Dispose()
        {
            MessageService.Message -= OnIncomingMessage;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task PostMessageAsync(string message)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
#if DEBUG
            Console.WriteLine($"{nameof(InjectableMessageService)}.{nameof(PostMessageAsync)}('{message}')");
#endif
            MessageService.PostMessage(message);
        }
    }
}
