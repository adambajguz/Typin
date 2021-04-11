namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Typin.Internal.Extensions;

    //TODO: maybe we should abstract help and version options, and remove nulablility of BindablePropertyInfo.Property?
    //TODO: maybye rename BindablePropertyInfo to BindableProperty?

    /// <summary>
    /// Represents a bindable <see cref="PropertyInfo"/>.
    /// </summary>
    public sealed class BindablePropertyInfo
    {
        /// <summary>
        /// Property info (may be null for built-in arguments, i.e., help and version options)
        /// </summary>
        public PropertyInfo? Property { get; }

        /// <summary>
        /// Property type (may be null for built-in arguments, i.e., help and version options)
        /// </summary>
        public Type? PropertyType => Property?.PropertyType;

        /// <summary>
        /// Property type (may be <see cref="string.Empty"/> for built-in arguments, i.e., help and version options)
        /// </summary>
        public string PropertyName => Property?.Name ?? string.Empty;

        /// <summary>
        /// Whether property is actually a built-in argument (help and version options).
        /// </summary>
        public bool IsBuiltIn => Property is null;

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
        /// Initializes an instance of <see cref="BindablePropertyInfo"/>.
        /// </summary>
        internal BindablePropertyInfo(PropertyInfo? property)
        {
            Property = property;
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
                if (IsBuiltIn)
                    return Array.Empty<string>();

                Type? underlyingType = PropertyType!.TryGetEnumerableUnderlyingType() ?? Property!.PropertyType;
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
        /// When <see cref="Property"/> is null, returns null.
        /// </summary>
        /// <param name="commandInstance">Command instance.</param>
        /// <returns>Property value.</returns>
        public object? GetValue(ICommand commandInstance)
        {
            return Property?.GetValue(commandInstance);
        }

        /// <summary>
        /// Sets a property value in command instance.
        /// When <see cref="Property"/> is null, call is ignored (no operation - nop).
        /// </summary>
        /// <param name="commandInstance">Command instance.</param>
        /// <param name="value">Value to set.</param>
        public void SetValue(ICommand commandInstance, object? value)
        {
            Property?.SetValue(commandInstance, value);
        }

        /// <summary>
        /// Converts <see cref="BindablePropertyInfo"/> to <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="bindablePropertyInfo">Bindable property info to convert.</param>
        public static implicit operator PropertyInfo?(BindablePropertyInfo bindablePropertyInfo)
        {
            return bindablePropertyInfo.Property;
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