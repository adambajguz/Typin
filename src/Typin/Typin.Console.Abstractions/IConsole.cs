namespace Typin.Console
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console.IO;

    /// <summary>
    /// Abstraction for interacting with the console.
    /// </summary>
    public interface IConsole : IStandardInput, IStandardOutputAndError
    {
        //TODO: add multiple consoles support
        //TODO: add AnsiConsole + ansi collors and commadns
        //TODO: add better console feature management (maybe add some enum with features supported by current instance)?

        /// <summary>
        /// Features supported by the console.
        /// </summary>
        ConsoleFeatures Features { get; }

        /// <summary>
        /// Current foreground color.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Current background color.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Sets a background RGB color.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        void SetBackground(byte r, byte g, byte b);

        /// <summary>
        /// Sets a background RGB color.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        void SetForeground(byte r, byte g, byte b);

        /// <summary>
        /// Sets a background and foreground RGB color.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="bg"></param>
        /// <param name="bb"></param>
        /// <param name="fr"></param>
        /// <param name="fb"></param>
        /// <param name="fg"></param>
        void SetColors(byte br, byte bg, byte bb,
                       byte fr, byte fg, byte fb);

        /// <summary>
        /// Clears the console.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        void Clear();

        /// <summary>
        /// Resets foreground and background color to default values.
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Cursor left offset.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        int CursorLeft { get; set; }

        /// <summary>
        /// Cursor top offset.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        int CursorTop { get; set; }

        /// <summary>
        /// Window width.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        int WindowWidth { get; set; }

        /// <summary>
        /// Window height.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        int WindowHeight { get; set; }

        /// <summary>
        /// Window buffer width.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        int BufferWidth { get; set; }

        /// <summary>
        /// Window buffer height.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        int BufferHeight { get; set; }

        /// <summary>
        /// Sets cursor position.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
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
        /// <param name="cancellationToken"></param>
        Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken cancellationToken = default);
    }
}