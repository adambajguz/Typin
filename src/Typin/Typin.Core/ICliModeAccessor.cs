namespace Typin
{
    using System;

    /// <summary>
    /// CLI mode accessor.
    /// </summary>
    public interface ICliModeAccessor
    {
        /// <summary>
        /// Current CLI mode type.
        /// </summary>
        Type? Type { get; }

        /// <summary>
        /// Current CLI mode instance.
        /// </summary>
        ICliMode? Instance { get; }
    }
}