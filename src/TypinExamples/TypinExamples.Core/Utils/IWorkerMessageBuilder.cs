namespace TypinExamples.Core.Utils
{
    using TypinExamples.Core.Models;

    public interface IWorkerMessageBuilder
    {
        bool WorkerMessageBuilt { get; }

        WorkerMessageModel Build();
    }
}