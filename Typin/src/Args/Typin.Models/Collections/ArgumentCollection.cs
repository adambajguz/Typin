namespace Typin.Models.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Typin.Models;

    /// <summary>
    /// Stores all arguments from the input.
    /// </summary>
    public sealed class ArgumentCollection : IArgumentCollection
    {
        private readonly Dictionary<string, ArgumentValue> _values;

        /// <inheritdoc/>
        public int Count => _values.Count;

        /// <summary>
        /// Initializes a new instace of <see cref="ArgumentCollection"/>.
        /// </summary>
        public ArgumentCollection()
        {
            _values = new();
        }

        /// <summary>
        /// Initializes a new instace of <see cref="ArgumentCollection"/>.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the internal dictionary can contain.</param>
        public ArgumentCollection(int capacity)
        {
            _values = new(capacity);
        }

        /// <summary>
        /// Initializes a new instace of <see cref="ArgumentCollection"/>.
        /// </summary>
        public ArgumentCollection(IEnumerable<KeyValuePair<string, ArgumentValue>> values)
        {
            _values = new Dictionary<string, ArgumentValue>(values);
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
        public ArgumentValue Get(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            if (_values.TryGetValue(propertyName, out ArgumentValue? value))
            {
                return value;
            }

            throw new KeyNotFoundException($"Cannot find dynamic property '{propertyName}' or the value was null.");
        }

        /// <inheritdoc/>
        public ArgumentValue? this[string propertyName]
        {
            get => GetOrDefault(propertyName);
            set => Set(propertyName, value);
        }

        /// <inheritdoc/>
        public ArgumentValue? GetOrDefault(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            return _values.GetValueOrDefault(propertyName);
        }

        /// <inheritdoc/>
        public void Set(string propertyName, ArgumentValue? value)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            if (value is null)
            {
                _values.Remove(propertyName);
            }

            _values[propertyName] = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc/>
        public void Set(string propertyName, object? value)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            if (_values.TryGetValue(propertyName, out ArgumentValue? previous))
            {
                _values[propertyName] = new ArgumentValue(previous.Schema, value);
            }
            else
            {
                throw new InvalidOperationException($"Property with name '{propertyName}' not found. Cannot set a schemaless property.");
            }
        }

        /// <inheritdoc/>
        public bool Remove(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            return _values.Remove(propertyName);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, ArgumentValue>> GetEnumerator()
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
    }
}