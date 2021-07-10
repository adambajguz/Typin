namespace Typin.Schemas
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Abstract command argument schema used in <see cref="ParameterSchema"/> and <see cref="OptionSchema"/>
    /// </summary>
    public abstract class ArgumentSchema
    {
        /// <summary>
        /// Bindable argument.
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
        /// Initializes an instance of <see cref="ArgumentSchema"/> that represents a property-based argument.
        /// </summary>
        protected ArgumentSchema(PropertyInfo property, string? description, Type? converterType)
        {
            Bindable = new BindableArgument(property);
            Description = description;
            ConverterType = converterType;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentSchema"/> that represents a dynamic or built-in argument.
        /// </summary>
        protected ArgumentSchema(Type propertyType, string propertyName, bool isDynamic, string? description, Type? converterType)
        {
            Bindable = new BindableArgument(propertyType, propertyName, isDynamic);
            Description = description;
            ConverterType = converterType;
        }
    }
}