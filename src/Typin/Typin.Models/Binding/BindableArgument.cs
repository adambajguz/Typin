namespace Typin.Models.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typin.Models;
    using Typin.Models.Collections;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Represents a bindable <see cref="PropertyInfo"/>.
    /// </summary>
    public sealed class BindableArgument : IBindableArgument
    {
        /// <summary>
        /// Property info (null for dynamic arguments).
        /// </summary>
        private PropertyInfo? Property { get; }

        /// <summary>
        /// Argument type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Enumerable argument underlying type.
        /// </summary>
        public Type? EnumerableUnderlyingType { get; }

        /// <summary>
        /// Argument type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Arguemnt type
        /// </summary>
        public BindableArgumentKind Kind { get; }

        /// <summary>
        /// Argument metadata.
        /// </summary>
        public IMetadataCollection Metadata { get; }

        /// <summary>
        /// Initializes an instance of <see cref="BindableArgument"/> that represents a property-based argument.
        /// </summary>
        internal BindableArgument(IMetadataCollection metadata, PropertyInfo property)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));

            Type = Property.PropertyType;
            EnumerableUnderlyingType = Type.TryGetEnumerableArgumentUnderlyingType();
            Name = Property.Name;
            Kind = BindableArgumentKind.Property;
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        /// <summary>
        /// Initializes an instance of <see cref="BindableArgument"/> that represents a built-in argument.
        /// </summary>
        internal BindableArgument(IMetadataCollection metadata, Type propertyType, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            Type = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            EnumerableUnderlyingType = Type.TryGetEnumerableArgumentUnderlyingType();
            Name = propertyName;
            Kind = BindableArgumentKind.Dynamic;
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
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
                Type? underlyingType = EnumerableUnderlyingType ?? Type;
                Type? nullableType = underlyingType.TryGetNullableUnderlyingType();
                underlyingType = nullableType ?? underlyingType;

                // Enum
                if (underlyingType.IsEnum)
                {
                    if (nullableType is null)
                    {
                        return Enum.GetNames(underlyingType);
                    }

                    List<string> enumNames = Enum.GetNames(underlyingType).ToList();
                    enumNames.Add(string.Empty);

                    return enumNames;
                }

                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Gets property value from command instance.
        /// </summary>
        /// <param name="instance">Model instance.</param>
        /// <returns>Property value.</returns>
        public object? GetValue(IModel instance)
        {
            if (Kind is BindableArgumentKind.Dynamic && instance is IDynamicModel dynamic)
            {
                return dynamic.Arguments.GetOrDefault(Name);
            }

            return Property?.GetValue(instance);
        }

        /// <summary>
        /// Sets a property value in model instance.
        /// </summary>
        /// <param name="instance">Model instance.</param>
        /// <param name="value">Value to set.</param>
        public void SetValue(IModel instance, object? value)
        {
            if (Kind is BindableArgumentKind.Dynamic && instance is IDynamicModel dynamic)
            {
                dynamic.Arguments.Set(Name, new ArgumentValue(Metadata, Type, value));
            }
            else
            {
                Property?.SetValue(instance, value);
            }
        }
    }
}