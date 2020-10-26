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
        public CliModeSwitcher ModeSwitcher { get; }

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
        public IEnumerable<ServiceDescriptor> Services { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> Middlewares => MiddlewareTypes;

        /// <summary>
        /// Collection of middlewares in application.
        /// </summary>
        internal LinkedList<Type> MiddlewareTypes { get; }

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
        public InputHistoryProvider InputHistory
        {
            get => inputHistoryProvider ?? throw new NullReferenceException("Input history is either uninitialized in this context or not available due to normal mode.");
            internal set => inputHistoryProvider = value;
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
                          ServiceCollection serviceCollection,
                          IConsole console,
                          LinkedList<Type> middlewareTypes)
        {
            ModeSwitcher = new CliModeSwitcher(this);
            Metadata = metadata;
            Configuration = applicationConfiguration;
            Services = serviceCollection;
            Console = console;
            MiddlewareTypes = middlewareTypes;
        }

        internal CliExecutionScope BeginExecutionScope(IServiceScopeFactory serviceScopeFactory)
        {
            return new CliExecutionScope(this, serviceScopeFactory);
        }
    }
}
