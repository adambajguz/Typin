namespace Typin.Binding
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base type for strongly-typed custom converters.
    /// </summary>
    public abstract class BindingConverter<T> : IBindingConverter
    {
        /// <inheritdoc/>
        Type IBindingConverter.TargetType { get; } = typeof(T);

        /// <inheritdoc/>
        object? IBindingConverter.Convert(string? value)
        {
            return Convert(value);
        }

        /// <inheritdoc/>
        object? IBindingConverter.Convert(IReadOnlyCollection<string> values)
        {
            return ConvertCollection(values);
        }

        /// <summary>
        /// Converts single line input value to <typeparamref name="T"/>.
        /// This method is used by scalar binder or non-scalar binder (when a property is a collection of <typeparamref name="T"/> - conversion for collection item type).
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted object instance.</returns>
        public abstract T? Convert(string? value);

        /// <summary>
        /// Converts multiple input values to <typeparamref name="T"/>.
        /// This method is used by non-scalar binder when converting collection type.
        /// </summary>
        /// <param name="values">Values to convert.</param>
        /// <returns>Converted object instance.</returns>
        public abstract T ConvertCollection(IReadOnlyCollection<string> values);
    }
}
