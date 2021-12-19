namespace Typin.Internal.Input
{
    using System;
    using System.Collections.Concurrent;
    using Typin.Binding;

    internal static class BindingConverterActivator
    {
        private static readonly ConcurrentDictionary<Type, IBindingConverter> _converters = new();

        public static IBindingConverter GetConverter(Type converterType)
        {
            return _converters.GetOrAdd(converterType, (ct) =>
            {
                return (IBindingConverter)Activator.CreateInstance(ct)!;
            });
        }
    }
}
