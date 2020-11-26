namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IWorkerMessageService
    {
        event EventHandler<string> IncomingMessage;

        Task PostMessageAsync(string message);
    }
}
