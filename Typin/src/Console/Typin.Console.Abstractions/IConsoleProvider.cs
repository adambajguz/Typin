namespace Typin.Console
{
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="IConsole"/> instance provider for accessing default console and named consoles.
    /// </summary>
    public interface IConsoleProvider
    {
        /// <summary>
        /// A collection of console names.
        /// </summary>
        IEnumerable<string> Names { get; }

        /// <summary>
        /// Default console.
        /// </summary>
        IConsole? Default { get; }

        /// <summary>
        /// Get a console by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The requested console, or null if it is not present.</returns>
        IConsole? this[string name] { get; }
    }
}
