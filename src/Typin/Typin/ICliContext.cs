namespace Typin
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Input;
    using Typin.Schemas;

    /// <summary>
    /// Command line application context.
    /// </summary>
    public interface ICliContext
    {
        /// <summary>
        /// Whether the application is running in interactive mode.
        /// </summary>
        bool IsInteractiveMode { get; }

        /// <summary>
        /// Current command scope in interactive mode.
        /// </summary>
        string Scope { get; set; }

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
        /// Service collection.
        /// </summary>
        IEnumerable<ServiceDescriptor> Services { get; }

        /// <summary>
        /// Collection of middlewares in application.
        /// </summary>
        IReadOnlyCollection<Type> Middlewares { get; }

        /// <summary>
        /// Console instance.
        /// </summary>
        IConsole Console { get; }

        /// <summary>
        /// Root schema.
        /// </summary>
        /// <exception cref="System.NullReferenceException"> Throws when uninitialized.</exception>
        RootSchema RootSchema { get; }

        /// <summary>
        /// Parsed CLI input.
        /// </summary>
        /// <exception cref="System.NullReferenceException"> Throws when uninitialized</exception>
        CommandInput Input { get; }

        /// <summary>
        /// Command input history in interactive mode.
        /// </summary>
        /// <exception cref="System.NullReferenceException"> Throws when uninitialized or in normal mode.</exception>
        InputHistoryProvider InputHistory { get; }

        /// <summary>
        /// Current command schema.
        /// </summary>
        /// <exception cref="System.NullReferenceException"> Throws when uninitialized.</exception>
        CommandSchema CommandSchema { get; }

        /// <summary>
        /// Current command instance.
        /// </summary>
        /// <exception cref="System.NullReferenceException"> Throws when not uninitialized.</exception>
        ICommand Command { get; }

        /// <summary>
        /// Collection of command's default values.
        /// </summary>
        /// <exception cref="System.NullReferenceException"> Throws when uninitialized.</exception>
        IReadOnlyDictionary<ArgumentSchema, object?> CommandDefaultValues { get; }

        /// <summary>
        /// Exit code from current command.
        /// Null if not set. If pipeline exits with null exit code it will be replaced with error exit code (1).
        /// </summary>
        int? ExitCode { get; set; }
    }
}
