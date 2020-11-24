namespace TypinExamples.Domain.Builders
{
    using System;
    using Newtonsoft.Json;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models.Workers;

    public sealed class WorkerMessageFromMainBuilder : WorkerMessageBuilder<WorkerMessageFromMainBuilder>
    {
        internal WorkerMessageFromMainBuilder() : base()
        {

        }

        /// <summary>
        /// Call worker command.
        /// </summary>
        public WorkerMessageFromMainBuilder CallCommand<T>(T data)
            where T : IWorkerRequest
        {
            TargetType = typeof(T);
            IsNotification = false;
            WorkerId = null;
            Data = data;

            return this;
        }

        /// <summary>
        /// Call worker command.
        /// </summary>
        public WorkerMessageFromMainBuilder CallCommand<T>(long workerId, T data)
            where T : IWorkerRequest
        {
            TargetType = typeof(T);
            IsNotification = false;
            WorkerId = workerId;
            Data = data;

            return this;
        }

        /// <summary>
        /// Send notification to worker.
        /// </summary>
        public WorkerMessageFromMainBuilder Notify<T>(long workerId, T data)
            where T : IWorkerNotification
        {
            TargetType = typeof(T);
            IsNotification = true;
            WorkerId = workerId;
            Data = data;

            return this;
        }

        /// <inheritdoc/>
        public override WorkerMessage Build()
        {
            if (WorkerMessageBuilt)
                throw new InvalidOperationException("Build can only be called once.");

            WorkerMessageBuilt = true;

            return new WorkerMessage
            {
                WorkerId = WorkerId,
                TargetType = TargetType?.AssemblyQualifiedName,
                IsNotification = IsNotification,
                Arguments = Arguments,
                Data = JsonConvert.SerializeObject(Data)
            };
        }
    }
}
