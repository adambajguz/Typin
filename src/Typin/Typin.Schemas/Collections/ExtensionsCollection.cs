namespace Typin.Schemas.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Default implementation for <see cref="IExtensionsCollection"/>.
    /// </summary>
    public class ExtensionsCollection : IExtensionsCollection
    {
        private readonly Dictionary<Type, object> _data = new();

        /// <summary>
        /// Initializes a new instance of <see cref="ExtensionsCollection"/>.
        /// </summary>
        public ExtensionsCollection()
        {

        }

        /// <inheritdoc />
        public object? this[Type key]
        {
            get
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                return _data.GetValueOrDefault(key);
            }
            set
            {
                _ = key ?? throw new ArgumentNullException(nameof(key));

                if (value is null)
                {
                    _data.Remove(key);

                    return;
                }

                _data[key] = value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        /// <inheritdoc />
        public TFeature? Get<TFeature>()
        {
            return (TFeature?)this[typeof(TFeature)];
        }

        /// <inheritdoc />
        public void Set<TFeature>(TFeature? instance)
        {
            this[typeof(TFeature)] = instance;
        }
    }
}