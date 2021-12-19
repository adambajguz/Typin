﻿namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    /// <summary>
    /// Abstract command argument schema used in <see cref="CommandParameterSchema"/> and <see cref="CommandOptionSchema"/>
    /// </summary>
    public abstract class ArgumentSchema
    {
        /// <summary>
        /// Bindable property info.
        /// </summary>
        public BindableArgument Bindable { get; }

        /// <summary>
        /// Command argument description, which is used in help text.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Binding converter type.
        /// </summary>
        public Type? ConverterType { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentSchema"/>.
        /// </summary>
        protected ArgumentSchema(PropertyInfo? property, string? description, Type? converterType)
        {
            Bindable = new BindableArgument(property);
            Description = description;
            ConverterType = converterType;
        }

        /// <summary>
        /// Property info may be null for built-in arguments (help and version options).
        /// </summary>
        [ExcludeFromCodeCoverage]
        [Obsolete("This property will be removed in Typin 4.0, instead use 'Bindable'.")]
        public PropertyInfo? Property => Bindable.Property;

        /// <summary>
        /// Whether command argument is scalar.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [Obsolete("This property will be removed in Typin 4.0, instead use 'Bindable.IsScalar'.")]
        public bool IsScalar => Bindable.IsScalar;

        /// <summary>
        /// Returns a list of valid values.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// The property's set accessor is not found.
        /// -or-
        /// value cannot be converted to the type of System.Reflection.PropertyInfo.PropertyType.</exception>
        /// <exception cref="TargetException">
        /// In the .NET for Windows Store apps or the Portable Class Library, catch System.Exception instead.
        /// The type of obj does not match the target type, or a property is an instance property but obj is null.
        /// </exception>
        /// <exception cref="MethodAccessException">
        /// In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, System.MemberAccessException, instead.
        /// There was an illegal attempt to access a private or protected method inside a class.
        /// </exception>
        /// <exception cref="TargetInvocationException">
        /// An error occurred while setting the property value.
        /// The System.Exception.InnerException property indicates the reason for the error.
        /// </exception>
        [ExcludeFromCodeCoverage]
        [Obsolete("This property will be removed in Typin 4.0, instead use 'Bindable.GetValidValues()'.")]
        public IReadOnlyList<string> GetValidValues()
        {
            return Bindable.GetValidValues();
        }
    }
}