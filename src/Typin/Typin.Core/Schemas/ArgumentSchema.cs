namespace Typin.Schemas
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Abstract command argument schema used in <see cref="CommandParameterSchema"/> and <see cref="CommandOptionSchema"/>
    /// </summary>
    public abstract class ArgumentSchema
    {
        /// <summary>
        /// Bindable property info.
        /// </summary>
        public BindablePropertyInfo BindableProperty { get; }

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
            BindableProperty = new BindablePropertyInfo(property);
            Description = description;
            ConverterType = converterType;
        }
    }
}