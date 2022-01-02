namespace Typin.Directives.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Typin.Directives;
    using Typin.Hosting;
    using Typin.Hosting.Scanning;
    using Typin.Models;
    using Typin.Models.Scanning;

    /// <summary>
    /// <see cref="IDirective"/> component scanner.
    /// </summary>
    internal sealed class AggregatedDirectiveScanner : IScanner<IDirective>, IDirectiveScanner
    {
        private readonly IDirectiveScanner _directiveScanner;
        private readonly IModelScanner _modelScanner;
        private readonly IConfigureDirectiveScanner _configureDirectiveScanner;

        /// <inheritdoc/>
        public Type ComponentType { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> Types => _directiveScanner.Types;

        /// <inheritdoc/>
        public event EventHandler<TypeAddedEventArgs>? Added
        {
            add => _directiveScanner.Added += value;
            remove => _directiveScanner.Added -= value;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveScanner"/>.
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="current"></param>
        public AggregatedDirectiveScanner(ICliBuilder cli, IEnumerable<Type>? current)
        {
            _directiveScanner = new DirectiveScanner(cli.Services, current);

            ComponentType = typeof(IDirective);

            _modelScanner = cli.AddModels(); // AddModels must be before ConfigureDirectives to ensure proper hosted services order
            _configureDirectiveScanner = cli.ConfigureDirectives();
        }

        /// <inheritdoc/>
        public bool IsValid(Type type)
        {
            return _directiveScanner.IsValid(type);
        }

        /// <inheritdoc/>
        public IScanner<IDirective> From(Assembly assembly)
        {
            _directiveScanner.From(assembly);
            _modelScanner.From(assembly);
            _configureDirectiveScanner.From(assembly);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<IDirective> From(IEnumerable<Assembly> assemblies)
        {
            _directiveScanner.From(assemblies);
            _modelScanner.From(assemblies);
            _configureDirectiveScanner.From(assemblies);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<IDirective> FromThisAssembly()
        {
            From(Assembly.GetCallingAssembly());

            return this;
        }

        /// <inheritdoc/>
        public IScanner<IDirective> Multiple(IEnumerable<Type> types)
        {
            _directiveScanner.Multiple(types);
            _modelScanner.Multiple(types);

            foreach (Type type in types)
            {
                _configureDirectiveScanner.FromNested(type);
            }

            return this;
        }

        /// <inheritdoc/>
        public IScanner<IDirective> Single(Type type)
        {
            _directiveScanner.Single(type);
            _modelScanner.Single(type);
            _configureDirectiveScanner.FromNested(type);

            return this;
        }

        IScanner<IDirective> IScanner<IDirective>.Single<T>()
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
