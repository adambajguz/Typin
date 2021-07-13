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
        /// Whether collection contains a metadata of type <paramref name="metadataType"/>.
        /// </summary>
        /// <param name="metadataType">Metadata type.</param>
        /// <returns>Whether collection contains a property.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="metadataType"/> is null.</exception>
        bool Contains(Type metadataType);

        /// <summary>
        /// Whether collection contains a metadata of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Metadata type.</typeparam>
        /// <returns>Whether collection contains a property.</returns>
        bool Contains<T>()
            where T : class, IArgumentMetadata;

        /// <summary>
        /// Gets metadata value.
        /// </summary>
        /// <param name="metadataType">Metadata type.</param>
        /// <returns>Metadata value or default when not found.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="metadataType"/> is null.</exception>
        IArgumentMetadata? GetValueOrDefault(Type metadataType);

        /// <summary>
        /// Gets metadata value.
        /// </summary>
        /// <typeparam name="T">Metadata type.</typeparam>
        /// <returns>Metadata value or default when not found.</returns>
        T? GetValueOrDefault<T>()
            where T : class, IArgumentMetadata;
    }
}