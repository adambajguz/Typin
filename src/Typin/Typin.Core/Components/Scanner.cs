namespace Typin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Internal.Extensions;

    /// <summary>
    /// CLI component scanner.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class Scanner<TComponent> : IScanner<TComponent>
        where TComponent : notnull
    {
        private readonly List<Type> _types = new();

        /// <summary>
        /// Services collection.
        /// </summary>
        protected IServiceCollection Services { get; }

        /// <summary>
        /// A component type.
        /// </summary>
        public Type ComponentType { get; }

        /// <summary>
        /// Added types to the block.
        /// </summary>
        public IReadOnlyList<Type> Types => _types;

        /// <summary>
        /// Initializes a new instance of <see cref="Scanner{TComponent}"/>.
        /// </summary>
        /// <param name="services"></param>
        protected Scanner(IServiceCollection services)
        {
            Services = services;
            ComponentType = typeof(TComponent);
        }

        /// <inheritdoc/>
        public virtual bool IsValidComponent(Type type)
        {
            return type.Implements(ComponentType) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Registers component service.
        /// </summary>
        /// <param name="type"></param>
        protected abstract void RegisterService(Type type);

        /// <inheritdoc/>
        public IScanner<TComponent> Single<T>()
            where T : class, TComponent
        {
            Single(typeof(T));

            return this;
        }

        /// <inheritdoc/>
        public IScanner<TComponent> Single(Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            if (!IsValidComponent(type))
            {
                throw new ArgumentException($"Type '{type.FullName}' does not match component type '{ComponentType.FullName}'.", nameof(type));
            }

            _types.Add(type);
            RegisterService(type);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<TComponent> Multiple(IEnumerable<Type> commandTypes)
        {
            foreach (Type commandType in commandTypes)
            {
                Single(commandType);
            }

            return this;
        }

        /// <inheritdoc/>
        public IScanner<TComponent> From(Assembly assembly)
        {
            foreach (Type type in assembly.ExportedTypes.Where(IsValidComponent))
            {
                _types.Add(type);
                RegisterService(type);
            }

            return this;
        }

        /// <inheritdoc/>
        public IScanner<TComponent> From(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                From(assembly);
            }

            return this;
        }

        /// <inheritdoc/>
        public IScanner<TComponent> FromThisAssembly()
        {
            From(Assembly.GetCallingAssembly());

            return this;
        }

        IScanner IScanner.Single(Type type)
        {
            return Single(type);
        }

        IScanner IScanner.Multiple(IEnumerable<Type> types)
        {
            return Multiple(types);
        }

        IScanner IScanner.From(Assembly assembly)
        {
            return From(assembly);
        }

        IScanner IScanner.From(IEnumerable<Assembly> assemblies)
        {
            return From(assemblies);
        }

        IScanner IScanner.FromThisAssembly()
        {
            return FromThisAssembly();
        }
    }
}
