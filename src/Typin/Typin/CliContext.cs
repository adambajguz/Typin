namespace Typin
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Schemas;

    /// <inheritdoc/>
    public class CliContext : ICliContext
    {
        private IReadOnlyDictionary<string, string>? environmentVariables;
        private RootSchema? rootSchema;
        private CommandInput? input;
        private InputHistoryProvider? inputHistoryProvider;
        private CommandSchema? commandSchema;
        private ICommand? command;
        private IReadOnlyDictionary<ArgumentSchema, object?>? commandDefaultValues;

        /// <inheritdoc/>
        [Obsolete("Use ModeSwitcher.Current instead of IsInteractiveMode. IsInteractiveMode will be removed in Typin 3.0.")]
        public bool IsInteractiveMode => ModeSwitcher.Current == CliModes.Interactive;

        /// <inheritdoc/>
        public ICliModeSwitcher ModeSwitcher { get; }

        /// <inheritdoc/>
        public string Scope { get; set; } = string.Empty;

        /// <inheritdoc/>
        public ApplicationMetadata Metadata { get; }

        /// <inheritdoc/>
        public ApplicationConfiguration Configuration { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> EnvironmentVariables
        {
            get => environmentVariables ?? throw new NullReferenceException("Environment variables are uninitialized in this context.");
            internal set => environmentVariables = value;
        }

        /// <inheritdoc/>
        [Obsolete("Use Configuration.Services instead of Services. Services will be removed in Typin 3.0.")]
        public IEnumerable<ServiceDescriptor> Services => Configuration.Services;

        /// <inheritdoc/>
        [Obsolete("Use Configuration.Middlewares instead of Middlewares. Middlewares will be removed in Typin 3.0.")]
        public IReadOnlyCollection<Type> Middlewares => Configuration.Middlewares;

        /// <inheritdoc/>
        public IConsole Console { get; }

        /// <inheritdoc/>
        public RootSchema RootSchema
        {
            get => rootSchema ?? throw new NullReferenceException("Root schema is uninitialized in this context.");
            internal set => rootSchema = value;
        }

        /// <inheritdoc/>
        public CommandInput Input
        {
            get => input ?? throw new NullReferenceException("Input is uninitialized in this context.");
            internal set => input = value;
        }

        /// <inheritdoc/>
        internal InputHistoryProvider InternalInputHistory
        {
            get => inputHistoryProvider ?? throw new NullReferenceException("Input history is either uninitialized in this context or not available due to direct mode.");
            set => inputHistoryProvider = value;
        }

        /// <inheritdoc/>
        public IInputHistoryProvider InputHistory
        {
            get => inputHistoryProvider ?? throw new NullReferenceException("Input history is either uninitialized in this context or not available due to direct mode.");
        }

        /// <inheritdoc/>
        public CommandSchema CommandSchema
        {
            get => commandSchema ?? throw new NullReferenceException("Current command schema is uninitialized in this context.");
            internal set => commandSchema = value;
        }

        /// <inheritdoc/>
        public ICommand Command
        {
            get => command ?? throw new NullReferenceException("Current command is uninitialized in this context.");
            internal set => command = value;
        }

        /// <inheritdoc/>
        public IReadOnlyDictionary<ArgumentSchema, object?> CommandDefaultValues
        {
            get => commandDefaultValues ?? throw new NullReferenceException("Current command default values is uninitialized in this context.");
            internal set => commandDefaultValues = value;
        }

        /// <inheritdoc/>
        public int? ExitCode { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="CliContext"/>.
        /// </summary>
        public CliContext(ApplicationMetadata metadata,
                          ApplicationConfiguration applicationConfiguration,
                          IConsole console)
        {
            ModeSwitcher = new CliModeSwitcher(this);
            Metadata = metadata;
            Configuration = applicationConfiguration;
            Console = console;
        }

        internal CliExecutionScope BeginExecutionScope(IServiceScopeFactory serviceScopeFactory)
        {
            return new CliExecutionScope(this, serviceScopeFactory);
        }
    }
}
