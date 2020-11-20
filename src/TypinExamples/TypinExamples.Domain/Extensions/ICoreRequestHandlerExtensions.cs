namespace TypinExamples.Domain.Extensions
{
    using TypinExamples.Domain.Builders;
    using TypinExamples.Domain.Interfaces.Handlers.Core;

    public static class ICoreRequestHandlerExtensions
    {
        public static WorkerMessageFromMainBuilder CreateMessageBuilder<TRequest, TResponse>(this ICoreRequestHandler<TRequest, TResponse> _)
            where TRequest : ICoreRequest<TResponse>
        {
            return WorkerMessageBuilder<WorkerMessageFromMainBuilder>.CreateFromMain();
        }
    }
}
