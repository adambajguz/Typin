namespace Typin.Console
{
    using System;
    using Typin.Console.IO;

    public partial interface IConsole : IStandardInput, IStandardOutputAndError
    {
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
        /// Sets buffer size.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        void SetBufferSize(int width, int height);
    }
}