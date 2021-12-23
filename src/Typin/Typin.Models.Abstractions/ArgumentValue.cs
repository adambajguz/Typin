namespace Typin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Typin.Models.Collections;

    /// <summary>
    /// Argument value.
    /// </summary>
    public sealed class ArgumentValue : IEquatable<ArgumentValue?>
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
        /// Initializes a new instance of <see cref="ArgumentValue"/>.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="expectedType"></param>
        /// <param name="value"></param>
        public ArgumentValue(IMetadataCollection metadata, Type expectedType, object? value)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            ExpectedType = expectedType;
            Value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ArgumentValue"/>.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="value"></param>
        public static ArgumentValue Create<T>(IMetadataCollection metadata, T value)
        {
            return new ArgumentValue(metadata, typeof(T), value);
        }

        /// <summary>
        /// Converts <see cref="Value"/> to <typeparamref name="T"/> or returns default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">Throws when failes to cast <see cref="Value"/> to <typeparamref name="T"/>.</exception>
        [return: MaybeNull]
        public T GetValue<T>()
        {
            if (Value == default)
            {
                return default;
            }

            return (T)Value;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as ArgumentValue);
        }

        /// <inheritdoc/>
        public bool Equals(ArgumentValue? other)
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
        public static bool operator ==(ArgumentValue? left, ArgumentValue? right)
        {
            return EqualityComparer<ArgumentValue?>.Default.Equals(left, right);
        }

        /// <summary>
        /// Determines whether two specified value objects have different values.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ArgumentValue? left, ArgumentValue? right)
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