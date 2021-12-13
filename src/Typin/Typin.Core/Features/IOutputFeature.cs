namespace Typin.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// Command line output feature.
    /// </summary>
    public interface IOutputFeature
    {
        /// <summary>
        /// Exit code from current command.
        /// Null if not set. If pipeline exits with null exit code it will be replaced with error exit code (1).
        /// </summary>
        int? ExitCode { get; set; }

        /// <summary>
        /// Optional data collection.
        /// </summary>
        Dictionary<string, object?>? Data { get; set; }
    }
}
