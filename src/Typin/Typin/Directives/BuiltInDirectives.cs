namespace Typin.Directives
{
    /// <summary>
    /// Standard command directives definitions.
    /// </summary>
    public static class BuiltInDirectives
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

        /// <summary>
        /// Normally when application runs in interactive mode, an empty line does nothing; but [!] will override this behavior, executing a root or scoped command.
        /// This directive will also force default command execution when input contains default commmand parameter values equal to command/subcommand name.
        /// </summary>
        public const string Default = "!";
    }
}
