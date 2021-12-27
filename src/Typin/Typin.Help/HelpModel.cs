namespace Typin.Help
{
    using Typin.Models;

    /// <summary>
    /// Encapsulates all help and version related options.
    /// </summary>
    public sealed class HelpModel : IModel
    {
        /// <summary>
        /// Whether help screen was rquested.
        /// </summary>
        [Option("help", 'h', Description = "Shows help.")]
        public bool ShowHelp { get; set; }

        /// <summary>
        /// Whether version info was requested.
        /// </summary>
        [Option("version", Description = "Shows application version.")]
        public bool ShowVersion { get; set; }
    }
}

