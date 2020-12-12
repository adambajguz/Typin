namespace Typin
{
    using System;
    using System.Collections.Generic;
    using Typin.Console;
    using Typin.Input;
    using Typin.Schemas;

    /// <summary>
    /// Command line application context.
    /// </summary>
    public interface ICliContext : IDisposable
    {
        /// <summary>
        /// Context instance id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Metadata associated with an application.
        /// </summary>
        ApplicationMetadata Metadata { get; }

        /// <summary>
        /// Configuration of an application.
        /// </summary>
        ApplicationConfiguration Configuration { get; }

        /// <summary>
        /// Collection of environment variables.
        /// </summary>
        IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

        /// <summary>
        /// Console instance.
        /// </summary>
        IConsole Console { get; }

        /// <summary>
        /// Root schema.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        RootSchema RootSchema { get; }

        /// <summary>
        /// Parsed CLI input.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized</exception>
        CommandInput Input { get; }

        /// <summary>
        /// Current command schema.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        CommandSchema CommandSchema { get; }

        /// <summary>
        /// Current command instance.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        ICommand Command { get; }

        /// <summary>
        /// Collection of command's default values.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        IReadOnlyDictionary<ArgumentSchema, object?> CommandDefaultValues { get; }

        /// <summary>
        /// Current command directives instances.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        IReadOnlyList<IDirective> Directives { get; }

        /// <summary>
        /// Current command pipelined directives instances.
        /// </summary>
        /// <exception cref="NullReferenceException"> Throws when uninitialized.</exception>
        IReadOnlyList<IPipelinedDirective> PipelinedDirectives { get; }

        /// <summary>
        /// Exit code from current command.
        /// Null if not set. If pipeline exits with null exit code it will be replaced with error exit code (1).
        /// </summary>
        int? ExitCode { get; set; }

        /// <summary>
        /// Finds and returns first directive instance of given type or null when not found.
        /// </summary>
        public IDirective? GetDirectiveInstance<T>() where T : IDirective;

        /// <summary>
        /// Finds and returns first directive instance of given type or null when not found.
        /// </summary>
        public IDirective? GetDirectiveInstance(Type type);

        /// <summary>
        /// Finds and returns directive instances of given type or empty collection when not found.
        /// </summary>
        public IEnumerable<IDirective> GetDirectiveInstances<T>() where T : IDirective;

        /// <summary>
        /// Finds and returns directive instances of given type or empty collection when not found.
        /// </summary>
        public IEnumerable<IDirective> GetDirectiveInstances(Type type);
    }
}
