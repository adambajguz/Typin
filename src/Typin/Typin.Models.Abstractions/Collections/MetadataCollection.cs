namespace Typin.Models.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Stores all metadata.
    /// </summary>
    public sealed class MetadataCollection : IMetadataCollection
    {
        private readonly IReadOnlyDictionary<Type, IArgumentMetadata> _values;

        /// <inheritdoc/>
        public int Count => _values.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <summary>
        /// Initializes a new instace of <see cref="MetadataCollection"/>.
        /// </summary>
        public MetadataCollection()
        {
            _values = new Dictionary<Type, IArgumentMetadata>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MetadataCollection"/> with the specified initial capacity.
        /// </summary>
        /// <param name="initialCapacity">The initial number of elements that the collection can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="initialCapacity"/> is less than 0</exception>
        public MetadataCollection(int initialCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }

            _values = new Dictionary<Type, IArgumentMetadata>(initialCapacity);
        }

        /// <inheritdoc />
        public IArgumentMetadata? this[Type key]
        {
            get
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                return _values.GetValueOrDefault(key);
            }
        }

        /// <summary>
        /// Initializes a new instace of <see cref="MetadataCollection"/>.
        /// </summary>
        public MetadataCollection(IEnumerable<KeyValuePair<Type, IArgumentMetadata>> values)
        {
            _values = new Dictionary<Type, IArgumentMetadata>(values);
        }

        /// <summary>
        /// Initializes a new instace of <see cref="MetadataCollection"/>.
        /// </summary>
        public MetadataCollection(IReadOnlyDictionary<Type, IArgumentMetadata> values)
        {
            _values = values;
        }

        /// <inheritdoc/>
        public IArgumentMetadata? Get(Type metadataType)
        {
            _ = metadataType ?? throw new ArgumentNullException(nameof(metadataType));

            return _values.GetValueOrDefault(metadataType);
        }

        /// <inheritdoc/>
        public T? Get<T>()
            where T : class, IArgumentMetadata
        {
            return _values.GetValueOrDefault(typeof(T)) as T;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.Values.GetEnumerator();
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _values.GetHashCode();
        }

        IEnumerator<IArgumentMetadata> IEnumerable<IArgumentMetadata>.GetEnumerator()
        {
            return _values.Values.GetEnumerator();
        }
    }
}