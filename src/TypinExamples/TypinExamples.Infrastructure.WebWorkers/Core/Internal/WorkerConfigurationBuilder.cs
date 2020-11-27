namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    internal class WorkerConfigurationBuilder : IWorkerConfigurationBuilder
    {
        private Type? _defaultEntryPoint;

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

        public WorkerConfiguration Build()
        {
            if (_defaultEntryPoint is null)
                throw new InvalidOperationException("When multiple entry points are registered default entry point must be set explicitly.");

            return new WorkerConfiguration(_defaultEntryPoint);
        }
    }
}
