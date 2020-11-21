namespace TypinExamples.Domain.Builders
{
    using TypinExamples.Domain.Models;

    public interface IWorkerMessageBuilder
    {
        bool WorkerMessageBuilt { get; }

        WorkerMessage Build();
    }
}