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
        /// Command line. <see cref="CommandLine"/> has a higher priority than <see cref="CommandLineArguments"/>.
        /// </summary>
        public string? CommandLine { get; set; }

        /// <summary>
        /// Command line arguments. <see cref="CommandLine"/> has a higher priority than <see cref="CommandLineArguments"/>.
        /// </summary>
        public string[]? CommandLineArguments { get; set; }

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
