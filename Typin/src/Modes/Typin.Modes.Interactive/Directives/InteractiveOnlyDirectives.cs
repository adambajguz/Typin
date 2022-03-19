namespace Typin.Modes.Interactive.Directives
{
    /// <summary>
    /// Interactive only directives definitions.
    /// </summary>
    public static class InteractiveOnlyDirectives
    {
        /// <summary>
        /// If application runs in interactive mode (using the [interactive] directive), it is possible to execute multiple commands in one processes.
        /// The application will run a in loop, constantly asking user for command input.
        /// This is useful for situations when it is necessary to execute multiple commands (since you don't have to constantly type dotnet ...).
        /// Furthermore, application context can be shared, which is useful when you have a db connection or startup takes very long.
        /// </summary>
        public const string Interactive = "interactive";
    }
}
