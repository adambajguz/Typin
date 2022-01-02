namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    using System;
    using System.Collections.Generic;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    internal static class MainConfiguration
    {
        private static readonly Dictionary<Type, MessageMapping> _messageMappings = new();
        public static IReadOnlyDictionary<Type, MessageMapping> MessageMappings => _messageMappings;

        internal static void AddMapping(MessageMapping mapping)
        {
            _messageMappings.Add(mapping.MessageType, mapping);
        }
    }
}
