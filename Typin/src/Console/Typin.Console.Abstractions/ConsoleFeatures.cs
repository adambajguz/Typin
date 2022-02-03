namespace Typin.Console
{
    using System;

    /// <summary>
    /// Console feature.
    /// </summary>
    [Flags]
    public enum ConsoleFeatures : ulong
    {
        /// <summary>
        /// Indicates that all optional features are supported/enabled.
        /// </summary>
        All = ConsoleColors | RgbColors | Clear | CursorPosition | CursorVisibility | WindowSize | BufferSize,

        /// <summary>
        /// Indicates that none of the optional features are supported/enabled.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.BackgroundColor"/>,
        /// <see cref="IConsole.ForegroundColor"/>,
        /// <see cref="IConsole.SetColors(ConsoleColor, ConsoleColor)"/>, and
        /// <see cref="IConsole.ResetColor"/>
        /// is enabled/alowed.
        /// </summary>
        ConsoleColors = 1ul << 1,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.SetBackground(byte, byte, byte)"/>,
        /// <see cref="IConsole.SetForeground(byte, byte, byte)"/>,
        /// <see cref="IConsole.SetColors(byte, byte, byte, byte, byte, byte)"/>, and
        /// <see cref="IConsole.ResetColor"/>
        /// is enabled/alowed.
        /// </summary>
        RgbColors = 1ul << 2,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.Clear"/>
        /// is enabled/alowed.
        /// </summary>
        Clear = 1ul << 3,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.CursorLeft"/>,
        /// <see cref="IConsole.CursorTop"/>, and
        /// <see cref="IConsole.SetCursorPosition(int, int)"/>
        /// are enabled/alowed.
        /// </summary>
        CursorPosition = 1ul << 10,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.CursorVisible"/>
        /// are enabled/alowed.
        /// </summary>
        CursorVisibility = 1ul << 11,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.WindowHeight"/>,
        /// <see cref="IConsole.WindowWidth"/>, and
        /// <see cref="IConsole.SetWindowSize(int, int)"/>.
        /// are enabled/alowed.
        /// </summary>
        WindowSize = 1ul << 20,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.BufferHeight"/>,
        /// <see cref="IConsole.BufferWidth"/>, and
        /// <see cref="IConsole.SetBufferSize(int, int)"/>.
        /// are enabled/alowed.
        /// </summary>
        BufferSize = 1ul << 21,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.Title"/>
        /// are enabled/alowed.
        /// </summary>
        Title = 1ul << 22
    }
}
