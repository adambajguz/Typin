namespace Typin.DynamicCommands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// <see cref="InputValue"/> extensions.
    /// </summary>
    public static class InputValueExtensions
    {
        /// <summary>
        /// Gets an argument value or a <paramref name="default"/>
        /// when <paramref name="inputValue"/> is null or argument's value is default.
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="default">Default value to use when <paramref name="inputValue"/> is null or argument's value is default.</param>
        /// <returns>Property value.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="default"/> is null.</exception>
        [return: NotNull]
        public static T GetValue<T>(this InputValue? inputValue, [DisallowNull] T @default)
        {
            _ = @default ?? throw new ArgumentNullException(nameof(@default));

            if (inputValue is null)
            {
                return @default;
            }

            T? value = inputValue.GetValue<T>();

            return EqualityComparer<T?>.Default.Equals(value, default) // This will match: null for classes, null(empty) for Nullable<T>, zero / false / etc for other structs
                ? @default
                : value ?? @default;
        }
    }
}