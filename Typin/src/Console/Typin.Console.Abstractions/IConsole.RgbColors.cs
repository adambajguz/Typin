namespace Typin.Console
{
    using System;
    using Typin.Console.IO;

    public partial interface IConsole : IStandardInput, IStandardOutputAndError
    {
        /// <summary>
        /// Sets a background RGB color.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        void SetBackground(byte r, byte g, byte b);

        /// <summary>
        /// Sets a background RGB color.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
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
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        void SetColors(byte br, byte bg, byte bb,
                       byte fr, byte fg, byte fb);
    }
}