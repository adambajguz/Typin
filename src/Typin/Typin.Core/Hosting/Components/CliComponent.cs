namespace Typin.Hosting.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Internal.Extensions;

    public abstract class CliComponent
    {
        private readonly List<Type> _types = new();

        /// <summary>
        /// Services collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// A component type.
        /// </summary>
        public Type ComponentType { get; }

        /// <summary>
        /// Added types to the block.
        /// </summary>
        public IReadOnlyList<Type> Types => _types;

        protected CliComponent(IServiceCollection services, Type holdingType)
        {
            Services = services;
            ComponentType = holdingType;
        }

        /// <summary>
        /// Whether block can hold the passed type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool CanHold(Type type)
        {
            return type.Implements(ComponentType) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        protected void Single(Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            if (!CanHold(type))
            {
                throw new ArgumentException(nameof(type), $"Type '{type.AssemblyQualifiedName}' does not match block type '{ComponentType.AssemblyQualifiedName}'.");
            }

            _types.Add(type);
            RegisterService(type);
        }

        protected void From(Assembly assembly)
        {
            foreach (Type type in assembly.ExportedTypes.Where(CanHold))
            {
                _types.Add(type);
            }
        }

        protected abstract void RegisterService(Type type);
    }

    public abstract class CliComponent<TComponent> : CliComponent, ICliComponent<TComponent>
        where TComponent : notnull
    {
        protected CliComponent(IServiceCollection services) : base(services, typeof(TComponent))
        {

        }

        /// <inheritdoc/>
        public ICliComponent<TComponent> Single<T>()
            where T : class, TComponent
        {
            base.Single(typeof(T));

            return this;
        }

        /// <inheritdoc/>
        public new ICliComponent<TComponent> Single(Type type)
        {
            base.Single(type);

            return this;
        }

        /// <inheritdoc/>
        public ICliComponent<TComponent> Multiple(IEnumerable<Type> commandTypes)
        {
            foreach (Type commandType in commandTypes)
            {
                Single(commandType);
            }

            return this;
        }

        /// <inheritdoc/>
        public new ICliComponent<TComponent> From(Assembly commandAssembly)
        {
            base.From(commandAssembly);

            return this;
        }

        /// <inheritdoc/>
        public ICliComponent<TComponent> From(IEnumerable<Assembly> commandAssemblies)
        {
            foreach (Assembly commandAssembly in commandAssemblies)
            {
                base.From(commandAssembly);
            }

            return this;
        }

        /// <inheritdoc/>
        public ICliComponent<TComponent> FromThisAssembly()
        {
            base.From(Assembly.GetCallingAssembly());

            return this;
        }
    }
}
