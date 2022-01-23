namespace Typin.Modes
{
    using System;

    /// <summary>
    /// Mode behavior.
    /// </summary>
    [Flags]
    public enum ModeBehavior
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