namespace Typin.Console
{
    /// <summary>
    /// Console provider options.
    /// </summary>
    public class ConsoleProviderOptions
    {
        /// <summary>
        /// Console name to console type map.
        /// </summary>
        public ConsoleCollection Consoles { get; } = new();
    }
}
