namespace Typin.Schemas.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Default implementation of <see cref="ISchemaCollection{TKey, TSchema}"/>.
    /// </summary>
    public class SchemaCollection<TKey, TSchema> : ISchemaCollection<TKey, TSchema>
        where TKey : notnull
        where TSchema : class, ISchema
    {
        /// <summary>
        /// Schema key accessor delegate.
        /// </summary>
        protected Func<TSchema, TKey> KeyAccessor { get; }

        /// <summary>
        /// Data.
        /// </summary>
        protected Dictionary<TKey, TSchema> Data { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of <see cref="SchemaCollection{TKey, TSchema}"/>.
        /// </summary>
        public SchemaCollection(Func<TSchema, TKey> keyAccessor)
        {
            KeyAccessor = keyAccessor;
        }

        /// <inheritdoc />
        public TSchema? this[TKey key]
        {
            get
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                return Data.GetValueOrDefault(key);
            }
            set
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                if (value is null)
                {
                    Data.Remove(key);

                    return;
                }

                Data[key] = value;
            }
        }

        /// <inheritdoc />
        public TSchema? Get(TKey key)
        {
            return this[key];
        }

        /// <inheritdoc />
        public void Set(TSchema instance)
        {
            TKey key = KeyAccessor(instance);

            this[key] = instance;
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));

            return Data.Remove(key);
        }

        /// <inheritdoc />
        public bool Remove(TSchema instance)
        {
            TKey key = KeyAccessor(instance);

            return Data.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TSchema>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }
}