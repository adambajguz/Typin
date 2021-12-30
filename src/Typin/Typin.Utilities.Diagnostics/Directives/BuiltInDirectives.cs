namespace Typin.Utilities.Diagnostics.Directives
{
    /// <summary>
    /// Names of diagnostics directives.
    /// </summary>
    public readonly struct DiagnosticsDirectives
    {
        /// <summary>
        /// When application runs in debug mode (using the [debug] directive), it will wait for debugger to be attached before proceeding.
        /// This is useful for debugging apps that were ran outside of the IDE.
        /// </summary>
        public const string Debug = "debug";

        /// <summary>
        /// When preview mode is specified (using the [preview] directive), the app will short-circuit by printing consumed command line arguments as they were parsed.
        /// This is useful when troubleshooting issues related to command routing and argument binding.
        /// </summary>
        public const string Preview = "preview";
    }
}
