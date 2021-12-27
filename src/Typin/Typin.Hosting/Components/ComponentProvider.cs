namespace Typin.Hosting.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// <see cref="IComponentProvider"/> implementation.
    /// </summary>
    internal sealed class ComponentProvider : IComponentProvider
    {
        private readonly IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> _components;

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> ComponentTypes { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ComponentProvider"/>.
        /// </summary>
        /// <param name="components"></param>
        public ComponentProvider(IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> components)
        {
            _components = components;
            ComponentTypes = components.Keys.ToHashSet();
        }

        /// <inheritdoc />
        public IReadOnlyCollection<Type>? this[Type key]
        {
            get
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                return _components.GetValueOrDefault(key);
            }
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type>? Get<T>()
        {
            return this[typeof(T)];
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type>? Get(Type componentType)
        {
            return this[componentType];
        }
    }
}