namespace Typin.Hosting.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// CLI component scanner.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class Scanner<TComponent> : IScanner<TComponent>
        where TComponent : notnull
    {
        private readonly HashSet<Type> _types;

        /// <inheritdoc/>
        public event EventHandler<TypeAddedEventArgs>? Added;

        /// <inheritdoc/>
        public Type ComponentType { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> Types => _types;

        /// <summary>
        /// Initializes a new instance of <see cref="Scanner{TComponent}"/>.
        /// </summary>
        /// <param name="services"></param>
        protected Scanner(IEnumerable<Type>? current)
        {
            _types = current is null
                ? new HashSet<Type>()
                : new HashSet<Type>(current);

            ComponentType = typeof(TComponent);
        }

        /// <inheritdoc/>
        public virtual bool IsValid(Type type)
        {
            return type.GetInterfaces().Contains(ComponentType) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Returns valid types from assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.ExportedTypes.Where(IsValid);
        }

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

            if (!IsValid(type))
            {
                throw new InvalidOperationException($"'{type}' is not a valid '{typeof(TComponent)}'.");
            }

            if (!_types.Contains(type))
            {
                _types.Add(type);
                Added?.Invoke(this, new TypeAddedEventArgs(type));
            }

            return this;
        }

        /// <inheritdoc/>
        public IScanner<TComponent> Multiple(IEnumerable<Type> types)
        {
            foreach (Type commandType in types)
            {
                Single(commandType);
            }

            return this;
        }

        /// <inheritdoc/>
        public IScanner<TComponent> From(Assembly assembly)
        {
            IEnumerable<Type> types = GetTypes(assembly);

            foreach (Type type in types)
            {
                if (!_types.Contains(type))
                {
                    _types.Add(type);
                    Added?.Invoke(this, new TypeAddedEventArgs(type));
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
