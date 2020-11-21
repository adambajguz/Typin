namespace TypinExamples.Domain.Builders
{
    using System;
    using System.Collections.Generic;
    using TypinExamples.Domain.Models;

    public abstract class WorkerMessageBuilder<TBuilder> : IWorkerMessageBuilder
        where TBuilder : class, IWorkerMessageBuilder
    {
        public bool WorkerMessageBuilt { get; protected set; }

        protected Type? TargetType { get; set; }
        protected bool IsNotification { get; set; }
        protected Dictionary<string, object> Arguments { get; } = new();
        protected object? Data { get; set; }
        protected long? WorkerId { get; set; } = null;

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

        public TBuilder AddArgument(string key, object value)
        {
            if (!Arguments.TryAdd(key, value))
            {
                Arguments.Remove(key);
                Arguments.Add(key, value);
            }

            return (this as TBuilder)!;
        }

        /// <summary>
        /// Builds message.
        /// </summary>
        public abstract WorkerMessage Build();
    }
}
