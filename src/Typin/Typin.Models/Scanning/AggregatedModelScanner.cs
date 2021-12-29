namespace Typin.Models.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Typin.Hosting;
    using Typin.Hosting.Scanning;
    using Typin.Models;

    /// <summary>
    /// <see cref="IModel"/> component scanner.
    /// </summary>
    internal sealed class AggregatedModelScanner : IScanner<IModel>, IModelScanner
    {
        private readonly IModelScanner _modelScanner;
        private readonly IConfigureModelScanner _configureModelScanner;

        /// <inheritdoc/>
        public Type ComponentType { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> Types => _modelScanner.Types;

        /// <inheritdoc/>
        public event EventHandler<TypeAddedEventArgs>? Added
        {
            add => _modelScanner.Added += value;
            remove => _modelScanner.Added -= value;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ModelScanner"/>.
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="current"></param>
        public AggregatedModelScanner(ICliBuilder cli, IEnumerable<Type>? current)
        {
            _modelScanner = new ModelScanner(cli.Services, current);

            ComponentType = typeof(IModel);

            _configureModelScanner = cli.ConfigureModels();
        }

        /// <inheritdoc/>
        public bool IsValidComponent(Type type)
        {
            return _modelScanner.IsValidComponent(type);
        }

        /// <inheritdoc/>
        public IScanner<IModel> From(Assembly assembly)
        {
            _modelScanner.From(assembly);
            _configureModelScanner.From(assembly);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<IModel> From(IEnumerable<Assembly> assemblies)
        {
            _modelScanner.From(assemblies);
            _configureModelScanner.From(assemblies);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<IModel> FromThisAssembly()
        {
            From(Assembly.GetCallingAssembly());

            return this;
        }

        /// <inheritdoc/>
        public IScanner<IModel> Multiple(IEnumerable<Type> types)
        {
            _modelScanner.Multiple(types);

            foreach (Type type in types)
            {
                _configureModelScanner.FromNested(type);
            }

            return this;
        }

        /// <inheritdoc/>
        public IScanner<IModel> Single(Type type)
        {
            _modelScanner.Single(type);
            _configureModelScanner.FromNested(type);

            return this;
        }

        IScanner<IModel> IScanner<IModel>.Single<T>()
        {
            return Single(typeof(T));
        }

        /// <inheritdoc/>
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

        IScanner IScanner.Multiple(IEnumerable<Type> types)
        {
            return Multiple(types);
        }

        IScanner IScanner.Single(Type type)
        {
            return Single(type);
        }
    }
}
