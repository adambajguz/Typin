namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    using System;
    using System.Collections.Generic;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Common.Payloads;
    using TypinExamples.Infrastructure.WebWorkers.Core.Internal.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    internal class WorkerConfigurationBuilder : IWorkerConfigurationBuilder
    {
        private Type? _defaultEntryPoint;
        private Dictionary<Type, MessageMapping> _messageMappings = new();

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

        public IWorkerConfigurationBuilder RegisterHandler<TPayload, THandler>()
            where THandler : IMessageHandler<TPayload>
        {
            Type messageType = typeof(Message<TPayload>);

            _messageMappings.TryAdd(messageType,
                                    new MessageMapping(messageType,
                                                       typeof(TPayload),
                                                       typeof(THandler),
                                                       typeof(IMessageHandler<TPayload>),
                                                       typeof(MessageHandlerWrapper<TPayload>)));

            return this;
        }

        public IWorkerConfigurationBuilder RegisterHandler<TPayload, THandler, TResultPayload>()
            where THandler : IMessageHandler<TPayload, TResultPayload>
        {
            Type messageType = typeof(Message<TPayload>);

            _messageMappings.TryAdd(messageType,
                                    new MessageMapping(messageType,
                                                       typeof(TPayload),
                                                       typeof(TResultPayload),
                                                       typeof(THandler),
                                                       typeof(IMessageHandler<TPayload, TResultPayload>),
                                                       typeof(MessageHandlerWrapper<TPayload, TResultPayload>)));

            return this;
        }

        public WorkerConfiguration Build()
        {
            if (_defaultEntryPoint is null)
                throw new InvalidOperationException("When multiple entry points are registered default entry point must be set explicitly.");

            RegisterHandler<Dispose.Payload, WorkerInstanceManager, Dispose.ResultPayload>();
            RegisterHandler<RunProgram.Payload, WorkerInstanceManager, RunProgram.ResultPayload>();
            RegisterHandler<Cancel.Payload, WorkerInstanceManager, Cancel.ResultPayload>();

            return new WorkerConfiguration(_defaultEntryPoint, _messageMappings);
        }
    }
}
