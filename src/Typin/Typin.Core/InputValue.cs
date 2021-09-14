namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Typin.Metadata;

    /// <summary>
    /// Input value.
    /// </summary>
    public sealed class InputValue : IEquatable<InputValue?>
    {
        /// <summary>
        /// Metadata.
        /// </summary>
        public IMetadataCollection Metadata { get; }

        /// <summary>
        /// Expected value type.
        /// </summary>
        public Type ExpectedType { get; }

        /// <summary>
        /// Bounded input value.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="InputValue"/>.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="expectedType"></param>
        /// <param name="value"></param>
        public InputValue(IMetadataCollection metadata, Type expectedType, object? value)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            ExpectedType = expectedType;
            Value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="InputValue"/>.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="value"></param>
        public InputValue Create<T>(IMetadataCollection metadata, T value)
        {
            return new InputValue(metadata, typeof(T), value);
        }

        /// <summary>
        /// Converts <see cref="Value"/> to <typeparamref name="T"/> or returns default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">Throws when failes to cast <see cref="Value"/> to <typeparamref name="T"/>.</exception>
        [return: MaybeNull]
        public T GetValueOrDefault<T>()
        {
            if (Value is null)
            {
                return default;
            }

            return (T)Value;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as InputValue);
        }

        /// <inheritdoc/>
        public bool Equals(InputValue? other)
        {
            return other != null &&
                   EqualityComparer<IMetadataCollection>.Default.Equals(Metadata, other.Metadata) &&
                   EqualityComparer<Type>.Default.Equals(ExpectedType, other.ExpectedType) &&
                   EqualityComparer<object?>.Default.Equals(Value, other.Value);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Metadata, ExpectedType, Value);
        }

        /// <summary>
        /// Determines whether two specified value objects have the same value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(InputValue? left, InputValue? right)
        {
            return EqualityComparer<InputValue?>.Default.Equals(left, right);
        }

        /// <summary>
        /// Determines whether two specified value objects have different values.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(InputValue? left, InputValue? right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }
    }
}