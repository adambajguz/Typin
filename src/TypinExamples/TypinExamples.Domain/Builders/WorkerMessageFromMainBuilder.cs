namespace TypinExamples.Domain.Builders
{
    using System;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models;

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
            return this;
        }

        /// <summary>
        /// Send notification to worker.
        /// </summary>
        public WorkerMessageFromMainBuilder Notify<T>()
            where T : IWorkerNotification
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
                Arguments = Arguments
            };
        }
    }
}
