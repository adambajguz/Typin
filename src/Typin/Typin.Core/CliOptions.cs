namespace Typin
{
    using System;
    using Typin.Console;

    /// <summary>
    /// CLI options.
    /// </summary>
    public sealed class CliOptions
    {
        /// <summary>
        /// Command line.
        /// </summary>
        public string? CommandLine { get; set; } //TODO: change to enumerable

        /// <summary>
        /// Startup <see cref="CommandLine"/> execution options.
        /// </summary>
        public CommandExecutionOptions StartupExecutionOptions { get; set; }

        /// <summary>
        /// Startup mode.
        /// </summary>
        public Type? StartupMode { get; set; }

        /// <summary>
        /// Startup message or null when no message.
        /// </summary>
        public Action<IServiceProvider, ApplicationMetadata, IConsole>? StartupMessage { get; set; }
    }
}
