namespace Typin.Models.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Argument metadata base class.
    /// </summary>
    public class ArgumentMetadata : IArgumentMetadata
    {
        /// <summary>
        /// Tags.
        /// </summary>
        public IReadOnlyList<string> Tags { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ArgumentMetadata"/>.
        /// </summary>
        /// <param name="tag"></param>
        public ArgumentMetadata(string tag)
        {
            _ = tag ?? throw new ArgumentNullException(nameof(tag));

            Tags = new[] { tag };
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ArgumentMetadata"/>.
        /// </summary>
        /// <param name="tags"></param>
        public ArgumentMetadata(IReadOnlyList<string> tags)
        {
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
        }
    }
}
