namespace Typin.Console
{
    using System;
    using Typin.Console.IO;

    /// <summary>
    /// Abstraction for interacting with the console.
    /// </summary>
    public partial interface IConsole : IStandardInput, IStandardOutputAndError
    {
        /// <summary>
        /// Features supported by the console.
        /// </summary>
        ConsoleFeatures SupportedFeatures { get; }

        /// <summary>
        /// Features supported by the console.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws when one or more requested features are not supported.</exception>
        ConsoleFeatures EnabledFeatures { get; set; }
    }
}