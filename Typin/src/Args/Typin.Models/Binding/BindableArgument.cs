namespace Typin.Models.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typin.Models;
    using Typin.Models.Collections;
    using Typin.Models.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Represents a bindable <see cref="PropertyInfo"/>.
    /// </summary>
    public sealed class BindableArgument : IBindableArgument
    {
        private readonly static Action<IDynamicModel, IArgumentCollection> _dynamicArgumentCollectionSetter;

        static BindableArgument()
        {
            MethodInfo methodInfo = typeof(IDynamicModel).GetProperty(nameof(IDynamicModel.Arguments))!.GetSetMethod(true)!;
            var @delegate = (Action<IDynamicModel, IArgumentCollection>)Delegate.CreateDelegate(typeof(Action<IDynamicModel, IArgumentCollection>), methodInfo);

            _dynamicArgumentCollectionSetter = @delegate;
        }

        /// <summary>
        /// Property info (null for dynamic arguments).
        /// </summary>
        private PropertyInfo? Property { get; }

        /// <inheritdoc/>
        public Type Type { get; }

        /// <inheritdoc/>
        public Type? EnumerableUnderlyingType { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public BindableArgumentKind Kind { get; }

        /// <inheritdoc/>
        public IArgumentSchema Schema { get; }

        /// <summary>
        /// Initializes an instance of <see cref="BindableArgument"/> that represents a property-based argument.
        /// </summary>
        internal BindableArgument(IArgumentSchema argumentSchema, PropertyInfo property)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));

            Type = Property.PropertyType;
            EnumerableUnderlyingType = Type.TryGetEnumerableArgumentUnderlyingType();
            Name = Property.Name;
            Kind = BindableArgumentKind.Property;
            Schema = argumentSchema;
        }

        /// <summary>
        /// Initializes an instance of <see cref="BindableArgument"/> that represents a built-in argument.
        /// </summary>
        internal BindableArgument(IArgumentSchema argumentSchema, Type propertyType, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            Type = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            EnumerableUnderlyingType = Type.TryGetEnumerableArgumentUnderlyingType();
            Name = propertyName;
            Kind = BindableArgumentKind.Dynamic;
            Schema = argumentSchema;
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
                if (dynamic.Arguments is null)
                {
                    _dynamicArgumentCollectionSetter.Invoke(dynamic, new ArgumentCollection());
                }

                return dynamic.Arguments?.GetOrDefault(Name);
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
                if (dynamic.Arguments is null)
                {
                    _dynamicArgumentCollectionSetter.Invoke(dynamic, new ArgumentCollection());
                }

                dynamic.Arguments!.Set(Name, new ArgumentValue(Schema, value));
            }
            else
            {
                Property?.SetValue(instance, value);
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Type)} = {Type}, " +
                $"{nameof(Kind)} = {Kind}, " +
                $"{nameof(Name)} = {Name}";
        }
    }
}