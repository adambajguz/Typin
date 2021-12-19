namespace Typin.Modes.Interactive.Directives
{
    /// <summary>
    /// Standard command directives definitions.
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

        /// <summary>
        /// When application runs in interactive mode, [>] directive followed by command(s) name(s) would scope to the command(s), allowing to ommit specified command name(s).
        /// <example>
        /// Commands:
        ///              > [>] cmd1 sub
        ///      cmd1 sub> list
        ///      cmd1 sub> get
        ///              > [>] cmd1
        ///          cmd1> test
        ///          cmd1> -h
        ///
        /// are an equivalent to:
        ///              > cmd1 sub list
        ///              > cmd1 sub get
        ///              > cmd1 test
        ///              > cmd1 -h
        /// </example>
        /// </summary>
        public const string Scope = ">";

        /// <summary>
        /// If application runs in interactive mode, [.] directive can be used to remove one command from the scope.
        /// <example>
        ///             > [>] cmd1 sub
        ///     cmd1 sub> list
        ///     cmd1 sub> [.]
        ///         cmd1>
        /// </example>
        /// </summary>
        public const string ScopeUp = ".";

        /// <summary>
        /// If application runs in interactive mode, [..] directive can be used to reset current scope to default (global scope).
        /// <example>
        ///             > [>] cmd1 sub
        ///     cmd1 sub> list
        ///     cmd1 sub> [..]
        ///             >
        /// </example>
        /// </summary>
        public const string ScopeReset = "..";
    }
}
