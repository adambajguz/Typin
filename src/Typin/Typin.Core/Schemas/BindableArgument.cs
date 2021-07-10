namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Represents a bindable <see cref="PropertyInfo"/>.
    /// </summary>
    public sealed class BindableArgument
    {
        /// <summary>
        /// Property info (null for dynamic arguments and built-in arguments, i.e., help and version options )
        /// </summary>
        public PropertyInfo? Property { get; }

        /// <summary>
        /// Arguemnt type.
        /// </summary>
        public Type Type { get; }

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
        public bool IsScalar
        {
            get
            {
                _isScalar ??= Type.TryGetEnumerableArgumentUnderlyingType() is null;

                return _isScalar.Value;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="BindableArgument"/> that represents a property-based argument.
        /// </summary>
        internal BindableArgument(PropertyInfo property)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Type = Property.PropertyType;
            Name = Property.Name;
            Kind = BindableArgumentKind.Property;
        }

        /// <summary>
        /// Initializes an instance of <see cref="BindableArgument"/> that represents a built-in argument.
        /// </summary>
        internal BindableArgument(Type propertyType, string propertyName, bool isDynamic)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            Type = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            Name = propertyName;
            Kind = isDynamic ? BindableArgumentKind.Dynamic : BindableArgumentKind.BuiltIn;
        }

        private bool? _isScalar;

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
                Type? underlyingType = Type.TryGetEnumerableUnderlyingType() ?? Type;
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
            if (commandInstance is IDynamicCommand dynamicCommandInstance)
            {
                return dynamicCommandInstance.Arguments.GetValueOrDefault(Name);
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
            if (commandInstance is IDynamicCommand dynamicCommandInstance)
            {
                dynamicCommandInstance.Arguments.SetValue(Name, value);
            }
            else
            {
                Property?.SetValue(commandInstance, value);
            }
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
        {
            return Property?.Equals(obj) ?? false;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Property?.GetHashCode() ?? 0;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string? ToString()
        {
            return Property?.ToString() ?? "<built-in argument>";
        }
    }
}