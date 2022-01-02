namespace Typin.Models.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a type for argument converters.
    /// </summary>
    public interface IArgumentConverter
    {
        /// <summary>
        /// The type to which input is converted.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Converts raw command line input to <see cref="object"/>.
        /// This method is used by argument binder.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted object instance.</returns>
        public object? Convert(string? value);

        /// <summary>
        /// Converts raw command line input to <see cref="object"/>.
        /// This method is used by argument binder.
        /// </summary>
        /// <param name="values">Values to convert.</param>
        /// <returns>Converted object instance.</returns>
        public object? Convert(IReadOnlyCollection<string> values);

        /// <summary>
        /// Checks whether type is a valid argument converter.
        /// </summary>
        public static bool IsValidType(Type type, Type propertyType)
        {
            Type[] interfaces = type.GetInterfaces();

            return interfaces.Contains(typeof(IArgumentConverter)) &&
                interfaces.Contains(typeof(IArgumentConverter<>).MakeGenericType(propertyType)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }

    /// <summary>
    /// Base type for strongly-typed argument converters.
    /// </summary>
    public interface IArgumentConverter<T> : IArgumentConverter
    {
        /// <inheritdoc/>
        Type IArgumentConverter.TargetType => typeof(T);

        /// <inheritdoc/>
        object? IArgumentConverter.Convert(string? value)
        {
            return Convert(value);
        }

        /// <inheritdoc/>
        object? IArgumentConverter.Convert(IReadOnlyCollection<string> values)
        {
            return ConvertCollection(values);
        }

        /// <summary>
        /// Converts single line input value to <typeparamref name="T"/>.
        /// This method is used by scalar binder or non-scalar binder (when a property is a collection of <typeparamref name="T"/> - conversion for collection item type).
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted object instance.</returns>
        new T? Convert(string? value);

        /// <summary>
        /// Converts multiple input values to <typeparamref name="T"/>.
        /// This method is used by non-scalar binder when converting collection type.
        /// </summary>
        /// <param name="values">Values to convert.</param>
        /// <returns>Converted object instance.</returns>
        T ConvertCollection(IReadOnlyCollection<string> values);

        /// <summary>
        /// Checks whether type is a valid argument converter.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            return type.GetInterfaces()
                .Contains(typeof(IArgumentConverter<T>)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}
