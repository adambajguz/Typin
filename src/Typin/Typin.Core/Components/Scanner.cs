namespace Typin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// CLI component scanner.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class Scanner<TComponent> : IScanner<TComponent>
        where TComponent : notnull
    {
        private readonly HashSet<Type> _types = new();

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
        public IReadOnlyCollection<Type> Types => _types;

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
        /// Registers component services.
        /// </summary>
        /// <param name="type"></param>
        protected abstract void RegisterServices(Type type);

        /// <summary>
        /// Implementation should provide with invalid component exception.
        /// </summary>
        /// <param name="type"></param>
        protected abstract Exception GetInvalidComponentException(Type type);

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
                throw GetInvalidComponentException(type);
            }

            if (!_types.Contains(type))
            {
                _types.Add(type);
                RegisterServices(type);
            }

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
                if (!_types.Contains(type))
                {
                    _types.Add(type);
                    RegisterServices(type);
                }
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
