namespace Typin.Plugins.Help
{
    /// <summary>
    /// <see cref="HelpHandler"/> options.
    /// </summary>
    public sealed class HelpHandlerOptions
    {
        /// <summary>
        /// Whether help screen is enabled (default: true).
        /// </summary>
        public bool HelpEnabled { get; set; } = true;

        /// <summary>
        /// Whether version info is enabled (default: true).
        /// </summary>
        public bool VersionEnabled { get; set; } = true;
    }
}