namespace Typin.Extensions
{
    /// <summary>
    /// <see cref="CliContext"/> extensions.
    /// </summary>
    public static class CliContextExtensions
    {
        /// <summary>
        /// Whether current context points to startup context.
        /// </summary>
        /// <param name="cliContext"></param>
        /// <returns></returns>
        public static bool IsStartupContext(this CliContext? cliContext)
        {
            return cliContext is null; //TODO: condition does not work
        }
    }
}
