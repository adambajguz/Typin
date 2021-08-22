namespace Typin.Internal.Components
{
    using System;
    using System.Collections.Generic;
    using Typin.Components;

    /// <summary>
    /// <see cref="ICliComponentProvider"/> implementation.
    /// </summary>
    internal sealed class CliComponentProvider : ICliComponentProvider
    {
        private readonly IReadOnlyDictionary<Type, IReadOnlyList<Type>> _cliComponents;

        public CliComponentProvider(IReadOnlyDictionary<Type, IReadOnlyList<Type>> cliComponents)
        {
            _cliComponents = cliComponents;
        }

        /// <inheritdoc/>
        public IReadOnlyList<Type> Get<T>()
        {
            return _cliComponents.GetValueOrDefault(typeof(T)) ?? Array.Empty<Type>();
        }

        /// <inheritdoc/>
        public IReadOnlyList<Type> Get(Type componentType)
        {
            return _cliComponents.GetValueOrDefault(componentType) ?? Array.Empty<Type>();
        }
    }
}