namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System;
    using System.Collections.Generic;

    public sealed class WorkerConfiguration
    {
        public Type ProgramType { get; }
        public IReadOnlyDictionary<Type, MessageMapping> MessageMappings { get; }

        public WorkerConfiguration(Type programType,
                                   IReadOnlyDictionary<Type, MessageMapping> messageMappings)
        {
            ProgramType = programType;
            MessageMappings = messageMappings;
        }
    }
}
