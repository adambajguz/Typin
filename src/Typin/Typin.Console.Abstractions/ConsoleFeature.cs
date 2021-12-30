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
        All = ConsoleColors | AnsiColors | Clear | CursorPosition | WindowDimensions | BufferDimensions | ReadKey,

        /// <summary>
        /// Indicates that none of the optional features are supported/enabled
        /// <see cref="IConsole.Clear"/>,
        /// is enabled/alowed.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.BackgroundColor"/>,
        /// <see cref="IConsole.ForegroundColor"/>,
        /// <see cref="IConsole.ResetColor"/>,
        /// is enabled/alowed.
        /// </summary>
        ConsoleColors = 1ul << 1,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.SetBackground(byte, byte, byte)"/>,
        /// is enabled/alowed.
        /// </summary>
        AnsiColors = 1ul << 2,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.Clear"/>,
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
        /// <see cref="IConsole.WindowHeight"/> and
        /// <see cref="IConsole.WindowWidth"/>
        /// are enabled/alowed.
        /// </summary>
        WindowDimensions = 1ul << 20,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.BufferHeight"/> and
        /// <see cref="IConsole.BufferWidth"/>
        /// are enabled/alowed.
        /// </summary>
        BufferDimensions = 1ul << 21,

        /// <summary>
        /// Indicates that
        /// <see cref="IConsole.ReadKey(bool)"/> and
        /// <see cref="IConsole.ReadKeyAsync(bool, System.Threading.CancellationToken)"/>
        /// are enabled/alowed.
        /// </summary>
        ReadKey = 1ul << 30
    }
}
