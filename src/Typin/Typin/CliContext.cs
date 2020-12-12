namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Console;
    using Typin.Input;
    using Typin.Schemas;

    /// <inheritdoc/>
    internal sealed class CliContext : ICliContext
    {
        private CommandInput? _input;
        private CommandSchema? _commandSchema;
        private IReadOnlyList<IDirective>? _directives;
        private IReadOnlyList<IPipelinedDirective>? _pipelinedDirectives;
        private ICommand? _command;
        private IReadOnlyDictionary<ArgumentSchema, object?>? _commandDefaultValues;

        /// <inheritdoc/>
        public Guid Id { get; } = Guid.NewGuid();

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
            get => _input ?? throw new NullReferenceException($"{nameof(Input)} is uninitialized in this context.");
            internal set => _input = value;
        }

        /// <inheritdoc/>
        public CommandSchema CommandSchema
        {
            get => _commandSchema ?? throw new NullReferenceException($"{nameof(CommandSchema)} is uninitialized in this context.");
            internal set => _commandSchema = value;
        }

        /// <inheritdoc/>
        public ICommand Command
        {
            get => _command ?? throw new NullReferenceException($"{nameof(Command)} is uninitialized in this context.");
            internal set => _command = value;
        }

        /// <inheritdoc/>
        public IReadOnlyDictionary<ArgumentSchema, object?> CommandDefaultValues
        {
            get => _commandDefaultValues ?? throw new NullReferenceException($"{nameof(CommandDefaultValues)} is uninitialized in this context.");
            internal set => _commandDefaultValues = value;
        }

        /// <inheritdoc/>
        public IReadOnlyList<IDirective> Directives
        {
            get => _directives ?? throw new NullReferenceException($"{nameof(Directives)} is uninitialized in this context.");
            internal set => _directives = value;
        }

        /// <inheritdoc/>
        public IReadOnlyList<IPipelinedDirective> PipelinedDirectives
        {
            get => _pipelinedDirectives ?? throw new NullReferenceException($"{nameof(PipelinedDirectives)} is uninitialized in this context.");
            internal set => _pipelinedDirectives = value;
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
            Metadata = metadata;
            Configuration = applicationConfiguration;
            RootSchema = rootSchema;
            EnvironmentVariables = environmentVariables;
            Console = console;
        }

        /// <inheritdoc/>
        public IDirective? GetDirectiveInstance<T>()
            where T : IDirective
        {
            return GetDirectiveInstance(typeof(T));
        }

        /// <inheritdoc/>
        public IDirective? GetDirectiveInstance(Type type)
        {
            return Directives.Where(x => x.GetType() == type).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<IDirective> GetDirectiveInstances<T>()
            where T : IDirective
        {
            return GetDirectiveInstances(typeof(T));
        }

        /// <inheritdoc/>
        public IEnumerable<IDirective> GetDirectiveInstances(Type type)
        {
            return Directives.Where(x => x.GetType() == type);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _input = default!;
            _command = default!;
            _commandSchema = default!;
            _commandDefaultValues = default!;
            ExitCode = null;
        }
    }
}
