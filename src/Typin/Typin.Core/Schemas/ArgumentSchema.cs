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
        /// Property info may be null for built-in arguments (help and version options)
        /// </summary>
        public PropertyInfo? Property { get; }

        /// <summary>
        /// Command argument description, which is used in help text.
        /// </summary>
        public string? Description { get; }

        private bool? _isScalar;

        /// <summary>
        /// Whether command argument is scalar.
        /// </summary>
        public bool IsScalar
        {
            get
            {
                _isScalar ??= Property.TryGetEnumerableArgumentUnderlyingType() is null;

                return _isScalar.Value;
            }
        }

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

        private IReadOnlyList<string>? _validValues;

        /// <summary>
        /// Returns a list of valid values.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<string> GetValidValues()
        {
            _validValues ??= InternalGetValidValues();

            return _validValues;

            IReadOnlyList<string> InternalGetValidValues()
            {
                if (Property is null)
                    return Array.Empty<string>();

                Type underlyingType = Property.PropertyType.TryGetNullableUnderlyingType() ??
                                      Property.PropertyType.TryGetEnumerableUnderlyingType() ??
                                      Property.PropertyType;

                // Enum
                if (underlyingType.IsEnum)
                    return Enum.GetNames(underlyingType);

                return Array.Empty<string>();
            }
        }
    }
}