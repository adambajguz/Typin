namespace TypinExamples.Domain.Extensions
{
    using TypinExamples.Core.Services;
    using TypinExamples.Domain.Builders;
    using TypinExamples.Workers.Services;

    public static class IWorkerServiceExtensions
    {
        public static WorkerMessageFromMainBuilder CreateMessageBuilder(this IWorkerTaskDispatcher _)
        {
            return WorkerMessageBuilder<WorkerMessageFromMainBuilder>.CreateFromMain();
        }

        public static WorkerMessageFromWorkerBuilder CreateMessageBuilder(this WorkerService _)
        {
            return WorkerMessageBuilder<WorkerMessageFromWorkerBuilder>.CreateFromWorker();
        }
    }
}
