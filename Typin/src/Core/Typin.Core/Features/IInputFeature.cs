namespace Typin.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// Command line input feature.
    /// </summary>
    public interface IInputFeature
    {
        /// <summary>
        /// Original raw command line input arguments.
        /// </summary>
        IEnumerable<string> Arguments { get; }

        /// <summary>
        /// Command execution options.
        /// </summary>
        InputOptions Options { get; }
    }
}
