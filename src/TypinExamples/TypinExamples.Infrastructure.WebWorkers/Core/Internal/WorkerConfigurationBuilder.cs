namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    using System;
    using System.Collections.Generic;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    internal class WorkerConfigurationBuilder : IWorkerConfigurationBuilder
    {
        private Type? _defaultEntryPoint;
        private readonly Dictionary<Type, MessageMapping> _messageMappings = new();

        /// <inheritdoc/>
        public IWorkerConfigurationBuilder UseProgram<T>()
            where T : class, IWorkerProgram
        {
            _defaultEntryPoint = typeof(T);

            return this;
        }

        /// <inheritdoc/>
        public IWorkerConfigurationBuilder UseLongRunningProgram()
        {
            _defaultEntryPoint = typeof(LongRunningWorkerProgram);

            return this;
        }

        public IWorkerConfigurationBuilder RegisterNotificationHandler<TPayload, THandler>()
            where THandler : INotificationHandler<TPayload>
        {
            Type messageType = typeof(Common.Messaging.Message<TPayload>);

            _messageMappings.TryAdd(messageType,
                                    new MessageMapping(messageType,
                                                       typeof(TPayload),
                                                       typeof(THandler),
                                                       typeof(INotificationHandler<TPayload>),
                                                       typeof(NotificationHandlerWrapper<TPayload>)));

            return this;
        }

        public IWorkerConfigurationBuilder RegisterCommandHandler<TPayload, THandler>()
            where THandler : ICommandHandler<TPayload>
        {
            Type messageType = typeof(Common.Messaging.Message<TPayload>);

            _messageMappings.TryAdd(messageType,
                                    new MessageMapping(messageType,
                                                       typeof(TPayload),
                                                       typeof(CommandFinished),
                                                       typeof(THandler),
                                                       typeof(ICommandHandler<TPayload>),
                                                       typeof(CommandHandlerWrapper<TPayload>)));

            return this;
        }

        public IWorkerConfigurationBuilder RegisterCommandHandler<TPayload, THandler, TResultPayload>()
            where THandler : ICommandHandler<TPayload, TResultPayload>
        {
            Type messageType = typeof(Common.Messaging.Message<TPayload>);

            _messageMappings.TryAdd(messageType,
                                    new MessageMapping(messageType,
                                                       typeof(TPayload),
                                                       typeof(TResultPayload),
                                                       typeof(THandler),
                                                       typeof(ICommandHandler<TPayload, TResultPayload>),
                                                       typeof(CommandHandlerWrapper<TPayload, TResultPayload>)));

            return this;
        }

        public WorkerConfiguration Build()
        {
            if (_defaultEntryPoint is null)
                throw new InvalidOperationException("When multiple entry points are registered default entry point must be set explicitly.");

            WorkerInstanceManager.ConfigureCoreHandlers(this);

            return new WorkerConfiguration(_defaultEntryPoint, _messageMappings);
        }
    }
}
