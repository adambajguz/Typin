namespace Typin.Modes.Interactive
{
    /// <summary>
    /// Interactive mode applicaiton buiilder extension settings.
    /// </summary>
    public sealed class InteractiveModeBuilderSettings
    {
        /// <summary>
        /// Whether to add [>], [.], and [..] directives (default: true).
        /// </summary>
        public bool AddScopeDirectives { get; init; } = true;

        /// <summary>
        /// Whether to add [interactive] directive (default: true).
        /// </summary>
        public bool AddInteractiveDirective { get; init; } = true;

        /// <summary>
        /// Whether to add interactive command (default: true).
        /// </summary>
        public bool AddInteractiveCommand { get; init; } = true;
    }
}
