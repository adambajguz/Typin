namespace Typin.Binding
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Internal type for custom converters.
    /// </summary>
    internal interface IBindingConverter
    {
        /// <summary>
        /// The type to which input is converted.
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Converts raw command line input to <see cref="object"/>.
        /// This method is used by argument binder.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted object instance.</returns>
        object? Convert(string? value);

        /// <summary>
        /// Converts raw command line input to <see cref="object"/>.
        /// This method is used by argument binder.
        /// </summary>
        /// <param name="values">Values to convert.</param>
        /// <returns>Converted object instance.</returns>
        object? Convert(IReadOnlyCollection<string> values);
    }
}
