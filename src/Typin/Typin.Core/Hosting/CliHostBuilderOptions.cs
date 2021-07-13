namespace Typin.Hosting
{
    using System.Collections.Generic;

    /// <summary>
    /// Builder options for use with <see cref="HostBuilderExtensions.ConfigureCliHost(IHostBuilder, Action{ICliHostBuilder}, Action{CliHostBuilderOptions})"/>.
    /// </summary>
    public class CliHostBuilderOptions
    {
        /// <summary>
        /// Command line (default: null).
        /// </summary>
        public string? CommandLineOverride { get; set; }

        /// <summary>
        /// Whether <see cref="CommandLineOverride"/> starts with executable name that should be ommited (default: false).
        /// </summary>
        public bool CommandLineOverrideStartsWithExecutableName { get; set; }

        /// <summary>
        /// Environemnt variables override (default: null).
        /// </summary>
        public IReadOnlyDictionary<string, string>? EnvironmentVariablesOverride { get; set; }
    }
}
