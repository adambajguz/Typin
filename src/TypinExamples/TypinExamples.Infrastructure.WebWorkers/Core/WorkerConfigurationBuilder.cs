namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WorkerConfigurationBuilder : IWorkerConfigurationBuilder
    {
        private Type? _defaultEntryPoint;
        private List<Type> _entryPoints = new();

        public IWorkerConfigurationBuilder UseProgram<T>(bool asDefault = true)
            where T : class, IWorkerProgram
        {
            if (asDefault)
                _defaultEntryPoint = typeof(T);

            _entryPoints.Add(typeof(T));

            return this;
        }

        public WorkerConfiguration Build()
        {
            if (_entryPoints.Count == 1 && _defaultEntryPoint is null)
            {
                _defaultEntryPoint = _entryPoints.First();
            }
            else if (_defaultEntryPoint is null)
                throw new InvalidOperationException("When multiple entry points are registered default entry point must be set explicitly.");

            return new WorkerConfiguration(_defaultEntryPoint, _entryPoints);
        }
    }
}
