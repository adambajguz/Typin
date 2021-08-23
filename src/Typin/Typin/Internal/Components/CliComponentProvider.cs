namespace Typin.Internal.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Components;

    /// <summary>
    /// <see cref="IComponentProvider"/> implementation.
    /// </summary>
    internal sealed class ComponentProvider : IComponentProvider
    {
        private readonly IReadOnlyDictionary<Type, IReadOnlyList<Type>> _cliComponents;

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> ComponentTypes { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ComponentProvider"/>.
        /// </summary>
        /// <param name="cliComponents"></param>
        public ComponentProvider(IReadOnlyDictionary<Type, IReadOnlyList<Type>> cliComponents)
        {
            _cliComponents = cliComponents;
            ComponentTypes = cliComponents.Keys.ToList();
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