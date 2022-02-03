namespace Typin.Console
{
    using System;
    using Typin.Console.IO;

    public partial interface IConsole : IStandardInput, IStandardOutputAndError
    {
        /// <summary>
        /// Current foreground color.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Current background color.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Sets console background and foregorund.
        /// </summary>
        /// <param name="background"></param>
        /// <param name="foreground"></param>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        void SetColors(ConsoleColor background, ConsoleColor foreground);

        /// <summary>
        /// Resets foreground and background color to default values.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        void ResetColor();
    }
}