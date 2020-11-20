namespace TypinExamples.Domain.Extensions
{
    using TypinExamples.Domain.Builders;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;

    public static class IWorkerRequestHandlerExtensions
    {
        public static WorkerMessageFromWorkerBuilder CreateMessageBuilder<TRequest>(this IWorkerRequestHandler<TRequest> _)
            where TRequest : IWorkerRequest
        {
            return WorkerMessageBuilder<WorkerMessageFromWorkerBuilder>.CreateFromWorker();
        }
    }
}
