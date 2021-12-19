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
        private readonly IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> _cliComponents;

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> ComponentTypes { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ComponentProvider"/>.
        /// </summary>
        /// <param name="cliComponents"></param>
        public ComponentProvider(IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> cliComponents)
        {
            _cliComponents = cliComponents;
            ComponentTypes = cliComponents.Keys.ToHashSet();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> Get<T>()
        {
            return _cliComponents.GetValueOrDefault(typeof(T)) ?? new HashSet<Type>();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> Get(Type componentType)
        {
            return _cliComponents.GetValueOrDefault(componentType) ?? new HashSet<Type>();
        }

        /// <summary>
        /// Merges <paramref name="componentProvider"/> with current instance and returns a new instance of <see cref="ComponentProvider"/>.
        /// </summary>
        /// <param name="componentProvider"></param>
        /// <returns></returns>
        public ComponentProvider Merge(ComponentProvider componentProvider)
        {
            var tmp = _cliComponents
                .Concat(componentProvider._cliComponents)
                .ToDictionary(x => x.Key, x => x.Value);

            return new ComponentProvider(tmp);
        }
    }
}