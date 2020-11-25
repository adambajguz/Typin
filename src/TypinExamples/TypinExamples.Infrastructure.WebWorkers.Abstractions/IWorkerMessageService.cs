namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore
{
    using System;
    using System.Threading.Tasks;

    public interface IWorkerMessageService
    {
        event EventHandler<string> IncomingMessage;

        Task PostMessageAsync(string message);
    }
}
