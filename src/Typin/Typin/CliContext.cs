namespace Typin
{
    using System;
    using System.Collections.Generic;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Input;
    using Typin.Schemas;

    /// <inheritdoc/>
    internal sealed class CliContext : ICliContext
    {
        private CommandInput? input;
        private InputHistoryProvider? inputHistoryProvider;
        private CommandSchema? commandSchema;
        private ICommand? command;
        private IReadOnlyDictionary<ArgumentSchema, object?>? commandDefaultValues;

        /// <inheritdoc/>
        public ICliModeSwitcher ModeSwitcher { get; }

        /// <inheritdoc/>
        public string Scope { get; set; } = string.Empty;

        /// <inheritdoc/>
        public ApplicationMetadata Metadata { get; }

        /// <inheritdoc/>
        public ApplicationConfiguration Configuration { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

        /// <inheritdoc/>
        public IConsole Console { get; }

        /// <inheritdoc/>
        public RootSchema RootSchema { get; set; }

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
        public IInputHistoryProvider InputHistory => inputHistoryProvider ?? throw new NullReferenceException("Input history is either uninitialized in this context or not available due to direct mode.");

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
                          RootSchema rootSchema,
                          IReadOnlyDictionary<string, string> environmentVariables,
                          IConsole console)
        {
            ModeSwitcher = new CliModeSwitcher(this);
            Metadata = metadata;
            Configuration = applicationConfiguration;
            RootSchema = rootSchema;
            EnvironmentVariables = environmentVariables;
            Console = console;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Input = default!;
            Command = default!;
            CommandDefaultValues = default!;
            CommandSchema = default!;
            ExitCode = null;
        }
    }
}
