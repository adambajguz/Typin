namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    using System;
    using System.Collections.Generic;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging.Handlers;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    internal class WorkerConfigurationBuilder : IWorkerConfigurationBuilder
    {
        private Type? _programType;
        private readonly Dictionary<Type, MessageMapping> _messageMappings = new();

        /// <inheritdoc/>
        public IWorkerConfigurationBuilder UseProgram<T>()
            where T : class, IWorkerProgram
        {
            _programType = typeof(T);

            return this;
        }

        public IWorkerConfigurationBuilder RegisterNotificationHandler<TNotification, THandler>()
            where THandler : INotificationHandler<TNotification>
            where TNotification : INotification
        {
            Type messageType = typeof(Message<TNotification>);

            _messageMappings.TryAdd(messageType,
                                    new MessageMapping(messageType,
                                                       typeof(TNotification),
                                                       typeof(THandler),
                                                       typeof(INotificationHandler<TNotification>),
                                                       typeof(NotificationHandlerWrapper<TNotification>)));

            return this;
        }

        public IWorkerConfigurationBuilder RegisterCommandHandler<TCommand, THandler>()
            where THandler : ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            Type messageType = typeof(Message<TCommand>);

            _messageMappings.TryAdd(messageType,
                                    new MessageMapping(messageType,
                                                       typeof(TCommand),
                                                       typeof(CommandFinished),
                                                       typeof(THandler),
                                                       typeof(ICommandHandler<TCommand>),
                                                       typeof(CommandHandlerWrapper<TCommand>)));

            return this;
        }

        public IWorkerConfigurationBuilder RegisterCommandHandler<TCommand, THandler, TResult>()
            where THandler : ICommandHandler<TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
            Type messageType = typeof(Message<TCommand>);

            _messageMappings.TryAdd(messageType,
                                    new MessageMapping(messageType,
                                                       typeof(TCommand),
                                                       typeof(TResult),
                                                       typeof(THandler),
                                                       typeof(ICommandHandler<TCommand, TResult>),
                                                       typeof(CommandHandlerWrapper<TCommand, TResult>)));

            return this;
        }

        public WorkerConfiguration Build()
        {
            _programType ??= typeof(LongRunningWorkerProgram);
            WorkerInstanceManager.ConfigureCoreHandlers(this);

            return new WorkerConfiguration(_programType, _messageMappings);
        }
    }
}
