namespace Typin.Commands
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
        /// Whether to ommit scope creartion.
        /// </summary>
        UseCurrentScope = 1 << 0
    }
}