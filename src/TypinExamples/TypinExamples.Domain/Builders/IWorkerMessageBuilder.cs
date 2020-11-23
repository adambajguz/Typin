namespace TypinExamples.Domain.Builders
{
    using TypinExamples.Domain.Models.Workers;

    public interface IWorkerMessageBuilder
    {
        bool WorkerMessageBuilt { get; }

        WorkerMessage Build();
    }
}