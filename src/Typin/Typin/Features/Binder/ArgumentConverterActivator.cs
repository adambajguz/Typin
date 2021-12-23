namespace Typin.Features.Binder
{
    using System;
    using System.Collections.Concurrent;
    using Typin.Models.Converters;

    internal static class ArgumentConverterActivator
    {
        private static readonly ConcurrentDictionary<Type, IArgumentConverter> _converters = new();

        public static IArgumentConverter GetConverter(Type converterType)
        {
            return _converters.GetOrAdd(converterType, (ct) =>
            {
                return (IArgumentConverter)Activator.CreateInstance(ct)!;
            });
        }
    }
}
