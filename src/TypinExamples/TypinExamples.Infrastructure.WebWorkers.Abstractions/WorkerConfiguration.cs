namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;

    public sealed class WorkerConfiguration
    {
        public Type ProgramType { get; }

        public WorkerConfiguration(Type programType)
        {
            ProgramType = programType;
        }
    }
}
