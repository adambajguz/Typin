namespace Typin.Features
{
    using System;

    /// <summary>
    /// Command line mode feature.
    /// </summary>
    public interface ICliModeFeature
    {
        /// <summary>
        /// Current CLI mode type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Current CLI mode instance.
        /// </summary>
        ICliMode Instance { get; }
    }
}
