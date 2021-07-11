namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typin.Internal.Extensions;
    using Typin.Metadata;

    /// <summary>
    /// Represents a bindable <see cref="PropertyInfo"/>.
    /// </summary>
    public sealed class BindableArgument
    {
        /// <summary>
        /// Property info (null for dynamic arguments and built-in arguments, i.e., help and version options )
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
        /// Whether command argument is scalar.
        /// </summary>
        public bool IsScalar => EnumerableUnderlyingType is null;

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
        internal BindableArgument(IMetadataCollection metadata, Type propertyType, string propertyName, bool isDynamic)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            Type = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            EnumerableUnderlyingType = Type.TryGetEnumerableArgumentUnderlyingType();
            Name = propertyName;
            Kind = isDynamic ? BindableArgumentKind.Dynamic : BindableArgumentKind.BuiltIn;
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
        /// For build-in arguemtns always returns null.
        /// </summary>
        /// <param name="commandInstance">Command instance.</param>
        /// <returns>Property value.</returns>
        public object? GetValue(ICommand commandInstance)
        {
            if (Kind == BindableArgumentKind.Dynamic && commandInstance is IDynamicCommand dynamicCommandInstance)
            {
                return dynamicCommandInstance.Arguments.TryGet(Name);
            }

            return Property?.GetValue(commandInstance);
        }

        /// <summary>
        /// Sets a property value in command instance.
        /// For build-in arguemtns call is always ignored (no operation - nop).
        /// </summary>
        /// <param name="commandInstance">Command instance.</param>
        /// <param name="value">Value to set.</param>
        public void SetValue(ICommand commandInstance, object? value)
        {
            if (Kind == BindableArgumentKind.Dynamic && commandInstance is IDynamicCommand dynamicCommandInstance)
            {
                dynamicCommandInstance.Arguments.Set(Name, new InputValue(Metadata, Type, value));
            }
            else
            {
                Property?.SetValue(commandInstance, value);
            }
        }
    }
}