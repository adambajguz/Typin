namespace Typin.Schemas.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of schemas.
    /// </summary>
    public interface ISchemaCollection<TKey0, TKey1, TSchema> : ISchemaCollection<TKey0, TSchema>
        where TKey0 : notnull, IEnumerable<TKey1>
        where TKey1 : notnull
        where TSchema : class, ISchema
    {
        /// <summary>
        /// Gets or sets a given schema. Setting a null value removes the schema.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested schema, or null if it is not present.</returns>
        TSchema? this[TKey1 key] { get; set; }

        /// <summary>
        /// Retrieves the requested schema from the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested schema, or null if it is not present.</returns>
        TSchema? Get(TKey1 key);

        /// <summary>
        /// Removes a schema in the collection by key.
        /// </summary>
        /// <param name="key">The schema key.</param>
        bool Remove(TKey1 key);
    }

    /// <summary>
    /// Represents a collection of schemas.
    /// </summary>
    public interface ISchemaCollection<TKey0, TSchema> : IEnumerable<KeyValuePair<TKey0, TSchema>>
        where TKey0 : notnull
        where TSchema : class, ISchema
    {
        /// <summary>
        /// Gets or sets a given schema. Setting a null value removes the schema.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested schema, or null if it is not present.</returns>
        TSchema? this[TKey0 key] { get; set; }

        /// <summary>
        /// Retrieves the requested schema from the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested schema, or null if it is not present.</returns>
        TSchema? Get(TKey0 key);

        /// <summary>
        /// Sets the given schema in the collection.
        /// </summary>
        /// <param name="instance">The schema value.</param>
        void Set(TSchema instance);

        /// <summary>
        /// Removes a schema in the collection by key.
        /// </summary>
        /// <param name="key">The schema key.</param>
        bool Remove(TKey0 key);

        /// <summary>
        /// Removes the given schema from the collection.
        /// </summary>
        /// <param name="instance">The schema value.</param>
        bool Remove(TSchema instance);
    }
}
