namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Abstract command argument schema used in <see cref="CommandParameterSchema"/> and <see cref="CommandOptionSchema"/>
    /// </summary>
    public abstract class ArgumentSchema
    {
        /// <summary>
        /// Property info can be null on built-in arguments (help and version options)
        /// </summary>
        public PropertyInfo? Property { get; }

        /// <summary>
        /// Command argument description, which is used in help text.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Whether command argument is scalar.
        /// </summary>
        public bool IsScalar => Property.TryGetEnumerableArgumentUnderlyingType() is null;

        /// <summary>
        /// Binding converter type.
        /// </summary>
        public Type? ConverterType { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentSchema"/>.
        /// </summary>
        protected ArgumentSchema(PropertyInfo? property, string? description, Type? converterType)
        {
            Property = property;
            Description = description;
            ConverterType = converterType;
        }

        /// <summary>
        /// Returns a list of valid values.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<string> GetValidValues()
        {
            if (Property is null)
                return Array.Empty<string>();

            Type underlyingType = Property.PropertyType.TryGetNullableUnderlyingType() ?? Property.PropertyType;

            // Enum
            if (underlyingType.IsEnum)
                return Enum.GetNames(underlyingType);

            return Array.Empty<string>();
        }
    }
}