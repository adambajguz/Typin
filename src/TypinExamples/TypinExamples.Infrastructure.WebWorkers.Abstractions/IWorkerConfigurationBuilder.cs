namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public interface IWorkerConfigurationBuilder
    {
        /// <summary>
        /// Use specific worker program class.
        /// </summary>
        IWorkerConfigurationBuilder UseProgram<TProgram>()
            where TProgram : class, IWorkerProgram;

        /// <summary>
        /// Use long running cancellable worker program class that will return 0 when it finishes.
        /// This can be used when you don't have any long running logic that you need to call and you only rely on messages.
        /// </summary>
        IWorkerConfigurationBuilder UseLongRunningProgram();

        /// <summary>
        /// Registers a handler.
        /// </summary>
        IWorkerConfigurationBuilder RegisterHandler<TPayload, THandler>()
            where THandler : IMessageHandler<TPayload>;

        /// <summary>
        /// Registers a handler.
        /// </summary>
        IWorkerConfigurationBuilder RegisterHandler<TPayload, THandler, TResultPayload>()
            where THandler : IMessageHandler<TPayload, TResultPayload>;
    }
}
