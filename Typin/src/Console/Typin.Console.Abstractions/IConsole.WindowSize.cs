namespace Typin.Console
{
    using System;
    using Typin.Console.IO;

    public partial interface IConsole : IStandardInput, IStandardOutputAndError
    {
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
        /// Gets the largest possible number of console window columns, based on the current font and screen resolution.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        int LargestWindowWidth { get; }

        /// <summary>
        /// Gets the largest possible number of console window rows, based on the current font and screen resolution.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        int LargestWindowHeight { get; }

        /// <summary>
        /// Sets window size.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        void SetWindowSize(int width, int height);
    }
}