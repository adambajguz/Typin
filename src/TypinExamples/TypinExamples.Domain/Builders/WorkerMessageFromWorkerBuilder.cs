namespace TypinExamples.Domain.Builders
{
    using System;
    using Newtonsoft.Json;
    using TypinExamples.Domain.Events;
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
        public WorkerMessageFromWorkerBuilder CallCommand<T>(T data)
            where T : ICoreRequest
        {
            TargetType = typeof(T);
            IsNotification = false;
            WorkerId = data.WorkerId;
            Data = data;

            return this;
        }

        /// <summary>
        /// Handle exception on core site.
        /// </summary>
        public WorkerMessageFromWorkerBuilder HandleException(long workerId, Exception ex)
        {
            TargetType = typeof(ExceptionFromWorkerNotification);
            Data = new ExceptionFromWorkerNotification
            {
                WorkerId = workerId,
                Type = ex.GetType().AssemblyQualifiedName ?? string.Empty,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };

            return this;
        }

        /// <summary>
        /// Send notification to core.
        /// </summary>
        public WorkerMessageFromWorkerBuilder Notify<T>(T data)
            where T : ICoreNotification
        {
            TargetType = typeof(T);
            Data = data;
            WorkerId = data.WorkerId ?? throw new ArgumentNullException("Cannot notify worker when worker id is null.");
            IsNotification = true;

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
                FromWorker = true,
                Arguments = Arguments,
                Data = JsonConvert.SerializeObject(Data)
            };
        }
    }
}
