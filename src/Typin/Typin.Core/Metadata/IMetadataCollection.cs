namespace Typin.Metadata
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Metadata collection.
    /// </summary>
    public interface IMetadataCollection : IReadOnlyCollection<IArgumentMetadata>
    {
        /// <summary>
        /// Indicates if the collection can be modified.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets metadata value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested feature, or null if it is not present.</returns>
        IArgumentMetadata? this[Type key] { get; }

        /// <summary>
        /// Gets metadata value.
        /// </summary>
        /// <param name="metadataType">Metadata type.</param>
        /// <returns>Metadata value or default when not found.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="metadataType"/> is null.</exception>
        IArgumentMetadata? Get(Type metadataType);

        /// <summary>
        /// Gets metadata value.
        /// </summary>
        /// <typeparam name="T">Metadata type.</typeparam>
        /// <returns>Metadata value or default when not found.</returns>
        T? Get<T>()
            where T : class, IArgumentMetadata;
    }
}