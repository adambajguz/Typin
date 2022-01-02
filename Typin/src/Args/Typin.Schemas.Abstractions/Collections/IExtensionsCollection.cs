namespace Typin.Schemas.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of extensions.
    /// </summary>
    public interface IExtensionsCollection : IEnumerable<KeyValuePair<Type, object>>
    {
        /// <summary>
        /// Gets or sets a given extension. Setting a null value removes the feature.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested extension, or null if it is not present.</returns>
        object? this[Type key] { get; set; }

        /// <summary>
        /// Retrieves the requested extension from the collection.
        /// </summary>
        /// <typeparam name="TExtension">The extension key.</typeparam>
        /// <returns>The requested extension, or null if it is not present.</returns>
        TExtension? Get<TExtension>();

        /// <summary>
        /// Sets the given extension in the collection.
        /// </summary>
        /// <typeparam name="TExtension">The extension key.</typeparam>
        /// <param name="instance">The extension value.</param>
        void Set<TExtension>(TExtension? instance);
    }
}
