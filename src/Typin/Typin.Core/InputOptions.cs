namespace Typin
{
    using System;

    /// <summary>
    /// Input options.
    /// </summary>
    [Flags]
    public enum InputOptions
    {
        /// <summary>
        /// Default command execution options without any special behavior.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Whether to trim executable name (first token) from arguments.
        /// </summary>
        TrimExecutable = 1 << 0,
    }
}
