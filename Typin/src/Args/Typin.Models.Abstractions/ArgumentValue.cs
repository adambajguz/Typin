namespace Typin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Typin.Models.Schemas;

    /// <summary>
    /// Argument value.
    /// </summary>
    public sealed class ArgumentValue : IEquatable<ArgumentValue?>
    {
        /// <summary>
        /// Argument schema.
        /// </summary>
        public IArgumentSchema Schema { get; }

        /// <summary>
        /// Bounded input value.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ArgumentValue"/>.
        /// </summary>
        /// <param name="argumentSchema"></param>
        /// <param name="value"></param>
        public ArgumentValue(IArgumentSchema argumentSchema, object? value)
        {
            Type? type = value?.GetType();
            if (type is not null && argumentSchema.Bindable.Type != type)
            {
                throw new ArgumentException($"Instance should be of type '{argumentSchema.Bindable.Type}' but is '{type}'", nameof(value));
            }

            Schema = argumentSchema ?? throw new ArgumentNullException(nameof(argumentSchema));
            Value = value;
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
                   EqualityComparer<IArgumentSchema>.Default.Equals(Schema, other.Schema) &&
                   EqualityComparer<object?>.Default.Equals(Value, other.Value);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Schema, Value);
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