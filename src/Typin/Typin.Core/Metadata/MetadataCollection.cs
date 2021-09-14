namespace Typin.Metadata
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Stores all metadata.
    /// </summary>
    public sealed class MetadataCollection : IMetadataCollection
    {
        /// <summary>
        /// Empty metadata collection instance.
        /// </summary>
        public static IMetadataCollection Empty { get; } = new MetadataCollection();

        private readonly IReadOnlyDictionary<Type, IArgumentMetadata> _values;

        /// <inheritdoc/>
        public int Count => _values.Count;

        /// <summary>
        /// Initializes a new instace of <see cref="MetadataCollection"/>.
        /// </summary>
        private MetadataCollection()
        {
            _values = new Dictionary<Type, IArgumentMetadata>();
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
        public bool Contains(Type metadataType)
        {
            _ = metadataType ?? throw new ArgumentNullException(nameof(metadataType));

            return _values.ContainsKey(metadataType);
        }

        /// <inheritdoc/>
        public bool Contains<T>()
            where T : class, IArgumentMetadata
        {
            return _values.ContainsKey(typeof(T));
        }

        /// <inheritdoc/>
        public IArgumentMetadata? GetValueOrDefault(Type metadataType)
        {
            _ = metadataType ?? throw new ArgumentNullException(nameof(metadataType));

            return _values.GetValueOrDefault(metadataType);
        }

        /// <inheritdoc/>
        public T? GetValueOrDefault<T>()
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