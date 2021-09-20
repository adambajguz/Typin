namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Input;
    using Typin.Schemas;

    /// <summary>
    /// Command line application context.
    /// </summary>
    public sealed class CliContext
    {
        /// <summary>
        /// Context instance id.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Parent <see cref="CliContext"/> instance.
        /// </summary>
        public CliContext? Parent { get; }

        /// <summary>
        /// CLI context depth.
        /// </summary>
        public int Depth { get; }

        /// <summary>
        /// Raw command line input arguments.
        /// </summary>
        public IEnumerable<string> Arguments { get; set; }

        /// <summary>
        /// Command execution options.
        /// </summary>
        public CommandExecutionOptions ExecutionOptions { get; set; }

        /// <summary>
        /// Parsed CLI input.
        /// </summary>
        public ParsedCommandInput? Input { get; set; }

        /// <summary>
        /// Current command schema.
        /// </summary>
        public CommandSchema? CommandSchema { get; set; }

        /// <summary>
        /// Current command instance.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        public ICommand? Command { get; set; }

        /// <summary>
        /// Collection of command's default values.
        /// </summary>
        public IReadOnlyDictionary<ArgumentSchema, object?>? CommandDefaultValues { get; set; }

        /// <summary>
        /// Current command directives instances.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        public IReadOnlyList<IDirective>? Directives { get; set; }

        /// <summary>
        /// Current command pipelined directives instances.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        public IReadOnlyList<IPipelinedDirective>? PipelinedDirectives { get; set; }

        /// <summary>
        /// Exit code from current command.
        /// Null if not set. If pipeline exits with null exit code it will be replaced with error exit code (1).
        /// </summary>
        public int? ExitCode { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="CliContext"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="arguments"></param>
        /// <param name="executionOptions"></param>
        public CliContext(CliContext? parent, IEnumerable<string> arguments, CommandExecutionOptions executionOptions)
        {
            Parent = parent;
            Arguments = arguments;
            ExecutionOptions = executionOptions;
            Depth = parent is null ? 0 : parent.Depth + 1;
        }

        /// <summary>
        /// Finds and returns first directive instance of given type or null when not found.
        /// </summary>
        public IDirective? GetDirectiveInstance<T>()
                  where T : IDirective
        {
            return GetDirectiveInstance(typeof(T));
        }

        /// <summary>
        /// Finds and returns first directive instance of given type or null when not found.
        /// </summary>
        public IDirective? GetDirectiveInstance(Type type)
        {
            return Directives?.Where(x => x.GetType() == type).FirstOrDefault();
        }

        /// <summary>
        /// Finds and returns directive instances of given type or empty collection when not found.
        /// </summary>
        public IEnumerable<IDirective> GetDirectiveInstances<T>()
            where T : IDirective
        {
            return GetDirectiveInstances(typeof(T));
        }

        /// <summary>
        /// Finds and returns directive instances of given type or empty collection when not found.
        /// </summary>
        public IEnumerable<IDirective> GetDirectiveInstances(Type type)
        {
            return Directives?.Where(x => x.GetType() == type) ?? Enumerable.Empty<IDirective>();
        }
    }
}
