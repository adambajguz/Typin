namespace TypinExamples.Domain.Builders
{
    using System;
    using TypinExamples.Domain.Interfaces.Handlers.Core;
    using TypinExamples.Domain.Models;

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
            return this;
        }

        /// <summary>
        /// Call core command.
        /// </summary>
        public WorkerMessageFromWorkerBuilder CallCommand<T>(T data)
            where T : ICoreRequest
        {
            CommandType = typeof(T);
            Data = data;
            return this;
        }

        /// <summary>
        /// Send notification to core.
        /// </summary>
        public WorkerMessageFromWorkerBuilder Notify<T>()
            where T : ICoreNotification
        {
            CommandType = typeof(T);
            return this;
        }

        /// <inheritdoc/>
        public override WorkerMessageModel Build()
        {
            if (WorkerMessageBuilt)
                throw new InvalidOperationException("Build can only be called once.");

            WorkerMessageBuilt = true;

            return new WorkerMessageModel
            {
                TargetCommandType = CommandType?.AssemblyQualifiedName,
                TargetNotificationType = NotificationType?.AssemblyQualifiedName,
                FromWorker = true,
                Arguments = Arguments
            };
        }
    }
}
