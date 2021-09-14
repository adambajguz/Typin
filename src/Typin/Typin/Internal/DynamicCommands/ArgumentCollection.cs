namespace Typin.Internal.DynamicCommands
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Typin;
    using Typin.DynamicCommands;
    using Typin.Metadata;

    /// <summary>
    /// Stores all arguments from the input.
    /// </summary>
    public sealed class ArgumentCollection : IArgumentCollection
    {
        private readonly Dictionary<string, InputValue> _values;

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
        public ArgumentCollection(IEnumerable<KeyValuePair<string, InputValue>> values)
        {
            _values = new Dictionary<string, InputValue>(values);
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
        public InputValue Get(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            if (_values.TryGetValue(propertyName, out InputValue? value))
            {
                return value;
            }

            throw new KeyNotFoundException($"Cannot find dynamic property '{propertyName}' or the value was null.");
        }

        /// <inheritdoc/>
        public InputValue this[string propertyName]
        {
            get => Get(propertyName);
            set => Set(propertyName, value);
        }

        /// <inheritdoc/>
        public InputValue? TryGet(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            return _values.GetValueOrDefault(propertyName);
        }

        /// <inheritdoc/>
        public void Set(string propertyName, InputValue value)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
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

            if (_values.TryGetValue(propertyName, out InputValue? previous))
            {
                _values[propertyName] = new InputValue(previous.Metadata, previous.ExpectedType, value);
            }
            else
            {
                _values[propertyName] = new InputValue(MetadataCollection.Empty, typeof(object), value);
            }
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, InputValue>> GetEnumerator()
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