namespace Typin.Console
{
    using System;
    using Typin.Console.IO;

    public partial interface IConsole : IStandardInput, IStandardOutputAndError
    {
        /// <summary>
        /// Gets or sets console title.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when not supported.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws when not supported on current platform.</exception>
        string Title { get; set; }
    }
}