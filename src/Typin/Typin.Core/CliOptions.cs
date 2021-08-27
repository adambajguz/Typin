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
        /// Command line (default: null).
        /// </summary>
        public string? CommandLine { get; set; }

        /// <summary>
        /// Whether <see cref="CommandLine"/> starts with executable name that should be ommited (default: false).
        /// </summary>
        public bool CommandLineStartsWithExecutableName { get; set; }

        /// <summary>
        /// Startup mode (default: null).
        /// </summary>
        public Type? StartupMode { get; set; }

        /// <summary>
        /// Startup message or null when no message (default: null).
        /// </summary>
        public Action<IServiceProvider, ApplicationMetadata, IConsole>? StartupMessage { get; set; }
    }
}
