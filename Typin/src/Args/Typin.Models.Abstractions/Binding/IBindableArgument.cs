namespace Typin.Models.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Typin.Models.Schemas;

    /// <summary>
    /// Represents a bindable argument.
    /// </summary>
    public interface IBindableArgument
    {
        /// <summary>
        /// Argument type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Enumerable argument underlying type.
        /// </summary>
        [MemberNotNullWhen(false, nameof(IsScalar))]
        Type? EnumerableUnderlyingType { get; }

        /// <summary>
        /// Argument type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Argument type
        /// </summary>
        BindableArgumentKind Kind { get; }

        /// <summary>
        /// Whether command argument is scalar.
        /// </summary>
        bool IsScalar => EnumerableUnderlyingType is null;

        /// <summary>
        /// Argument schema.
        /// </summary>
        IArgumentSchema Schema { get; }

        /// <summary>
        /// Returns a list of valid values.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<string> GetValidValues();

        /// <summary>
        /// Gets argument value from model instance.
        /// </summary>
        /// <param name="instance">Command instance.</param>
        /// <returns>Property value.</returns>
        object? GetValue(IModel instance);

        /// <summary>
        /// Sets a property value in model instance.
        /// </summary>
        /// <param name="instance">Model instance.</param>
        /// <param name="value">Value to set.</param>
        void SetValue(IModel instance, object? value);
    }
}