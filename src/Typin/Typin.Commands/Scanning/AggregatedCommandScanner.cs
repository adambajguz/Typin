namespace Typin.Commands.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Typin.Commands;
    using Typin.Hosting;
    using Typin.Hosting.Scanning;
    using Typin.Models;

    /// <summary>
    /// <see cref="ICommand"/> component scanner.
    /// </summary>
    internal sealed class AggregatedCommandScanner : IScanner<ICommand>, ICommandScanner
    {
        private readonly ICliBuilder _cli;
        private readonly ICommandScanner _commandScanner;

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
            _cli = cli;
            _commandScanner = new CommandScanner(cli.Services, current);

            ComponentType = typeof(ICommand);
        }

        /// <inheritdoc/>
        public bool IsValidComponent(Type type)
        {
            return _commandScanner.IsValidComponent(type);
        }

        /// <inheritdoc/>
        public IScanner<ICommand> From(Assembly assembly)
        {
            _commandScanner.From(assembly);

            _cli.ConfigureCommands().From(assembly);
            _cli.ConfigureModels().From(assembly);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<ICommand> From(IEnumerable<Assembly> assemblies)
        {
            _commandScanner.From(assemblies);

            _cli.ConfigureCommands().From(assemblies);
            _cli.ConfigureModels().From(assemblies);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<ICommand> FromThisAssembly()
        {
            _commandScanner.FromThisAssembly();

            _cli.ConfigureCommands().FromThisAssembly();
            _cli.ConfigureModels().FromThisAssembly();

            return this;
        }

        /// <inheritdoc/>
        public IScanner<ICommand> Multiple(IEnumerable<Type> types)
        {
            _commandScanner.Multiple(types);

            _cli.ConfigureCommands().Multiple(types);
            _cli.ConfigureModels().Multiple(types);

            return this;
        }

        /// <inheritdoc/>
        public IScanner<ICommand> Single(Type type)
        {
            _commandScanner.Single(type);

            _cli.ConfigureCommands().Single(type);
            _cli.ConfigureModels().Single(type);

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
