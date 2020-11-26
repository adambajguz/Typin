namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Collections.Generic;

    public class WorkerConfiguration
    {
        public Type DefaultEntryPoint { get; }
        public IReadOnlyList<Type> EntryPoints { get; }

        public WorkerConfiguration(Type defaultEntryPoint, IReadOnlyList<Type> entryPoints)
        {
            DefaultEntryPoint = defaultEntryPoint;
            EntryPoints = entryPoints;
        }
    }
}
