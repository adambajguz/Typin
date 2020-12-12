namespace Typin.Console
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console.IO;

    /// <summary>
    /// Abstraction for interacting with the console.
    /// </summary>
    public interface IConsole : IStandardInput, IStandardOutputAndError, IDisposable
    {
        /// <summary>
        /// Current foreground color.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Current background color.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Clears the console.
        /// </summary>
        void Clear();

        /// <summary>
        /// Resets foreground and background color to default values.
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Cursor left offset.
        /// </summary>
        int CursorLeft { get; set; }

        /// <summary>
        /// Cursor top offset.
        /// </summary>
        int CursorTop { get; set; }

        /// <summary>
        /// Window width.
        /// </summary>
        int WindowWidth { get; set; }

        /// <summary>
        /// Window height.
        /// </summary>
        int WindowHeight { get; set; }

        /// <summary>
        /// Window buffer width.
        /// </summary>
        int BufferWidth { get; set; }

        /// <summary>
        /// Window buffer height.
        /// </summary>
        int BufferHeight { get; set; }

        /// <summary>
        /// Defers the application termination in case of a cancellation request and returns the token that represents it.
        /// Subsequent calls to this method return the same token.
        /// </summary>
        /// <remarks>
        /// When working with System.Console:<br/>
        /// - Cancellation can be requested by the user by pressing Ctrl+C.<br/>
        /// - Cancellation can only be deferred once, subsequent requests to cancel by the user will result in instant termination.<br/>
        /// - Any code executing prior to calling this method is not cancellation-aware and as such will terminate instantly when cancellation is requested.
        /// </remarks>
        CancellationToken GetCancellationToken();

        /// <summary>
        /// Sets cursor position.
        /// </summary>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false);
    }
}