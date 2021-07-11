namespace Typin.DynamicCommands
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Stores all arguemtns frmo the input.
    /// </summary>
    public sealed class DynamicArgumentCollection : IDynamicArgumentCollection
    {
        private readonly Dictionary<string, object?> _values;

        /// <inheritdoc/>
        public int Count => _values.Count;

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicArgumentCollection"/>.
        /// </summary>
        public DynamicArgumentCollection()
        {
            _values = new();
        }

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicArgumentCollection"/>.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the internal dictionary can contain.</param>
        public DynamicArgumentCollection(int capacity)
        {
            _values = new(capacity);
        }

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicArgumentCollection"/>.
        /// </summary>
        public DynamicArgumentCollection(IEnumerable<KeyValuePair<string, object?>> values)
        {
#if NETSTANDARD2_0
            _values = System.Linq.Enumerable.ToDictionary(values, x => x.Key, x => x.Value);
#else
            _values = new Dictionary<string, object?>(values);
#endif
        }

        /// <inheritdoc/>
        public bool Contains(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            return _values.ContainsKey(propertyName);
        }

        /// <inheritdoc/>
        public object? GetValueOrDefault(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            return _values.GetValueOrDefault(propertyName);
        }

        /// <inheritdoc/>
        public T? GetValueOrDefault<T>(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            object? value = _values.GetValueOrDefault(propertyName);

            if (value is null)
            {
                return default;
            }

            return (T)value;
        }

        /// <inheritdoc/>
        public object? GetValue(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            if (_values.TryGetValue(propertyName, out object? value))
            {
                return value;
            }

            throw new NullReferenceException($"Cannot find dynamic property '{propertyName}' or the value was null.");
        }

        /// <inheritdoc/>
        public T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            if (_values.TryGetValue(propertyName, out object? value))
            {
                return (T)value!;
            }

            throw new NullReferenceException($"Cannot find dynamic property '{propertyName}' or the value was null.");
        }

        /// <inheritdoc/>
        public void SetValue(string propertyName, object? value)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            _values[propertyName] = value;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as DynamicArgumentCollection);
        }

        /// <inheritdoc/>
        public bool Equals(DynamicArgumentCollection? other)
        {
            return other != null &&
                   EqualityComparer<Dictionary<string, object?>>.Default.Equals(_values, other._values);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _values.GetHashCode();
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return _values.Values.GetEnumerator();
        }
    }
}