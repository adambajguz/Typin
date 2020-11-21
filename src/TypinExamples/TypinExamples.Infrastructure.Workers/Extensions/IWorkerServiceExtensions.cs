namespace TypinExamples.Infrastructure.Workers.Extensions
{
    using TypinExamples.Application.Services;
    using TypinExamples.Domain.Builders;
    using TypinExamples.Infrastructure.Workers.WorkerServices;

    public static class IWorkerServiceExtensions
    {
        public static WorkerMessageFromMainBuilder CreateMessageBuilder(this IWorkerMessageDispatcher _)
        {
            return WorkerMessageBuilder<WorkerMessageFromMainBuilder>.CreateFromMain();
        }

        public static WorkerMessageFromWorkerBuilder CreateMessageBuilder(this WorkerService _)
        {
            return WorkerMessageBuilder<WorkerMessageFromWorkerBuilder>.CreateFromWorker();
        }
    }
}
