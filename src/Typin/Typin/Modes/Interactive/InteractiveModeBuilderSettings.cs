namespace Typin.Modes
{
    /// <summary>
    /// Interactive mode applicaiton buiilder extension settings.
    /// </summary>
    public sealed class InteractiveModeBuilderSettings
    {
        /// <summary>
        /// Whether to add [>], [.], and [..] directives.
        /// </summary>
        public bool AddScopeDirectives { get; set; } = true;

        /// <summary>
        /// Whether to add [interactive] directive.
        /// </summary>
        public bool AddInteractiveDirective { get; set; } = true;

        /// <summary>
        /// Whether to add interactive command.
        /// </summary>
        public bool AddInteractiveCommand { get; set; } = true;
    }
}
