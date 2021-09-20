namespace Typin
{
    using System;

    /// <summary>
    /// Command execution options.
    /// </summary>
    [Flags]
    public enum CommandExecutionOptions
    {
        /// <summary>
        /// Default command execution options without any special behavior.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Whether to trim executable name (first token) from arguments.
        /// </summary>
        TrimExecutable = 1 << 0,

        /// <summary>
        /// Whether to ommit scope creartion.
        /// </summary>
        UseCurrentScope = 1 << 1
    }
}