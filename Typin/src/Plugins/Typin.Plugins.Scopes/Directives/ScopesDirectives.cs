namespace Typin.Plugins.Scopes.Directives
{
    /// <summary>
    /// Scopes directives definitions.
    /// </summary>
    public static class ScopesDirectives
    {
        /// <summary>
        /// When application runs in interactive mode, [>] directive followed by command(s) name(s) would scope to the command(s), allowing to omit specified command name(s).
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
