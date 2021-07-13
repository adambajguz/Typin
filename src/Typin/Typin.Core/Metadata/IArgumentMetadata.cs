namespace Typin.Metadata
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents argument metadata.
    /// </summary>
    public interface IArgumentMetadata
    {
        /// <summary>
        /// Tags.
        /// </summary>
        IReadOnlyList<string> Tags { get; }
    }
}
