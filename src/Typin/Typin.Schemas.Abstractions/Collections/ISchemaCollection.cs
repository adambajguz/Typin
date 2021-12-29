namespace Typin.Schemas.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of schemas.
    /// </summary>
    public interface ISchemaCollection<TKey, TSchema> : IEnumerable<KeyValuePair<TKey, TSchema>>
        where TKey : notnull
        where TSchema : class, ISchema
    {
        /// <summary>
        /// Gets or sets a given schema. Setting a null value removes the feature.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested schema, or null if it is not present.</returns>
        TSchema? this[TKey key] { get; set; }

        /// <summary>
        /// Retrieves the requested schema from the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested schema, or null if it is not present.</returns>
        TSchema? Get(TKey key);

        /// <summary>
        /// Sets the given schema in the collection.
        /// </summary>
        /// <param name="instance">The schema value.</param>
        void Set(TSchema instance);

        /// <summary>
        /// Removes a schema in the collection by key.
        /// </summary>
        /// <param name="key">The schema key.</param>
        bool Remove(TKey key);

        /// <summary>
        /// Removes the given schema from the collection.
        /// </summary>
        /// <param name="instance">The schema value.</param>
        bool Remove(TSchema instance);
    }
}
