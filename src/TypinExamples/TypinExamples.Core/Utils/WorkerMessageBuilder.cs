namespace TypinExamples.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using TypinExamples.Core.Handlers.Core;
    using TypinExamples.Core.Handlers.Workers;
    using TypinExamples.Core.Models;

    public abstract class WorkerMessageBuilder<TBuilder> : IWorkerMessageBuilder
        where TBuilder : class, IWorkerMessageBuilder
    {
        public bool WorkerMessageBuilt { get; protected set; }

        protected Type? CommandType { get; set; }
        protected Type? NotificationType { get; set; }
        protected Dictionary<string, object> Arguments { get; } = new();

        protected WorkerMessageBuilder()
        {

        }

        public static WorkerMessageFromMainBuilder CreateFromMain()
        {
            return new WorkerMessageFromMainBuilder();
        }

        public static WorkerMessageFromWorkerBuilder CreateFromWorker()
        {
            return new WorkerMessageFromWorkerBuilder();
        }

        public TBuilder AddArgument()
        {
            return (this as TBuilder)!;
        }

        /// <summary>
        /// Builds message.
        /// </summary>
        public abstract WorkerMessageModel Build();
    }

    public sealed class WorkerMessageFromMainBuilder : WorkerMessageBuilder<WorkerMessageFromMainBuilder>
    {
        internal WorkerMessageFromMainBuilder() : base()
        {

        }

        /// <summary>
        /// Call worker command.
        /// </summary>
        public WorkerMessageFromMainBuilder CallCommand<T>()
            where T : IWorkerRequest
        {
            CommandType = typeof(T);
            return (this as WorkerMessageFromMainBuilder)!;
        }

        /// <summary>
        /// Send notification to worker.
        /// </summary>
        public WorkerMessageFromMainBuilder Notify<T>()
            where T : IWorkerNotification
        {
            CommandType = typeof(T);
            return (this as WorkerMessageFromMainBuilder)!;
        }

        /// <inheritdoc/>
        public override WorkerMessageModel Build()
        {
            if (WorkerMessageBuilt)
                throw new InvalidOperationException("Build can only be called once.");

            WorkerMessageBuilt = true;

            return new WorkerMessageModel
            {
                Command = CommandType?.AssemblyQualifiedName,
                Notification = NotificationType?.AssemblyQualifiedName,
            };
        }
    }

    public sealed class WorkerMessageFromWorkerBuilder : WorkerMessageBuilder<WorkerMessageFromWorkerBuilder>
    {
        internal WorkerMessageFromWorkerBuilder() : base()
        {

        }

        /// <summary>
        /// Call core command.
        /// </summary>
        public WorkerMessageFromWorkerBuilder CallCommand<T>()
            where T : ICoreRequest
        {
            CommandType = typeof(T);
            return (this as WorkerMessageFromWorkerBuilder)!;
        }

        /// <summary>
        /// Send notification to core.
        /// </summary>
        public WorkerMessageFromWorkerBuilder Notify<T>()
            where T : ICoreNotification
        {
            CommandType = typeof(T);
            return (this as WorkerMessageFromWorkerBuilder)!;
        }

        /// <inheritdoc/>
        public override WorkerMessageModel Build()
        {
            if (WorkerMessageBuilt)
                throw new InvalidOperationException("Build can only be called once.");

            WorkerMessageBuilt = true;

            return new WorkerMessageModel
            {
                Command = CommandType?.AssemblyQualifiedName,
                Notification = NotificationType?.AssemblyQualifiedName,
                FromWorker = true,
            };
        }
    }
}
