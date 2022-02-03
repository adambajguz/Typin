namespace Typin.Console.IO
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Key reader that provides a analogue to
    /// <see cref="Console.ReadKey()"/> and <see cref="Console.ReadKey(bool)"/>
    /// for <see cref="StandardStreamReader"/>.
    /// </summary>
    public interface IKeyReader
    {
        /// <summary>
        /// Obtains the next character or function key pressed by the user.
        /// The pressed key is optionally displayed in the console window.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>
        /// Obtains the next character or function key pressed by the user asynchronously.
        /// The pressed key is optionally displayed in the console window.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken cancellationToken = default);
    }
}
