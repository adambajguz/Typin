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
        IWorkerConfigurationBuilder RegisterNotificationHandler<TNotification, THandler>()
            where THandler : INotificationHandler<TNotification>
            where TNotification : INotification;

        /// <summary>
        /// Registers a handler.
        /// </summary>
        IWorkerConfigurationBuilder RegisterCommandHandler<TCommand, THandler>()
            where THandler : ICommandHandler<TCommand>
            where TCommand : ICommand;

        /// <summary>
        /// Registers a handler.
        /// </summary>
        IWorkerConfigurationBuilder RegisterCommandHandler<TCommand, THandler, TResult>()
            where THandler : ICommandHandler<TCommand, TResult>
            where TCommand : ICommand<TResult>;
    }
}
