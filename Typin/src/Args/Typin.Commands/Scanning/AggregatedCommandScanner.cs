namespace Typin.Commands.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Typin.Commands;
    using Typin.Hosting;
    using Typin.Hosting.Scanning;
    using Typin.Models;
    using Typin.Models.Scanning;

    /// <summary>
    /// <see cref="ICommand"/> component scanner.
    /// </summary>
    internal sealed class AggregatedCommandScanner : IScanner<ICommand>, ICommandScanner
    {
        private readonly ICommandScanner _commandScanner;
        private readonly IModelScanner _modelScanner;
        private readonly IConfigureCommandScanner _configureCommandScanner;

        /// <inheritdoc/>
        public Type ComponentType { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> Types => _commandScanner.Types;

        /// <inheritdoc/>
        public event EventHandler<TypeAddedEventArgs>? Added
        {
            add => _commandScanner.Added += value;
            remove => _commandScanner.Added -= value;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandScanner"/>.
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="current"></param>
        public AggregatedCommandScanner(ICliBuilder cli, IEnumerable<Type>? current)
        {
            _commandScanner = new CommandScanner(cli.Services, current);

            ComponentType = typeof(ICommand);

            _modelScanner = cli.AddModels(); // AddModels must be before ConfigureCommands to ensure proper hosted services order
            _configureCommandScanner = cli.ConfigureCommands();
        }

        /// <inheritdoc/>
        public bool IsValid(Type type)
        {
            return _commandScanner.IsValid(type);
        }

        /// <inheritdoc/>
        public IScanner<ICommand> From(Assembly assembly)
        {
            _commandScanner.From(assembly);
            _modelScanner.From(assembly);
            _configureCommandScanner.From(assembly);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<ICommand> From(IEnumerable<Assembly> assemblies)
        {
            _commandScanner.From(assemblies);
            _modelScanner.From(assemblies);
            _configureCommandScanner.From(assemblies);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<ICommand> FromThisAssembly()
        {
            From(Assembly.GetCallingAssembly());

            return this;
        }

        /// <inheritdoc/>
        public IScanner<ICommand> Multiple(IEnumerable<Type> types)
        {
            _commandScanner.Multiple(types);
            _modelScanner.Multiple(types);

            foreach (Type type in types)
            {
                _configureCommandScanner.FromNested(type);
            }

            return this;
        }

        /// <inheritdoc/>
        public IScanner<ICommand> Single(Type type)
        {
            _commandScanner.Single(type);
            _modelScanner.Single(type);
            _configureCommandScanner.FromNested(type);

            return this;
        }

        IScanner<ICommand> IScanner<ICommand>.Single<T>()
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
