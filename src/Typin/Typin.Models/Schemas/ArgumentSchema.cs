namespace Typin.Models.Schemas
{
    using System;
    using System.Reflection;
    using Typin.Models.Binding;
    using Typin.Models.Collections;

    /// <summary>
    /// Abstract command argument schema used in <see cref="ParameterSchema"/> and <see cref="OptionSchema"/>
    /// </summary>
    public abstract class ArgumentSchema : IArgumentSchema
    {
        /// <inheritdoc/>
        public IBindableArgument Bindable { get; }

        /// <inheritdoc/>
        public string? Name { get; }

        /// <inheritdoc/>
        public string? Description { get; }

        /// <inheritdoc/>
        public Type? ConverterType { get; init; }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentSchema"/> that represents a property-based argument.
        /// </summary>
        protected ArgumentSchema(PropertyInfo property,
                                 string? name,
                                 string? description,
                                 Type? converterType,
                                 IMetadataCollection metadata)
        {
            Bindable = new BindableArgument(metadata, property);
            Name = name;
            Description = description;
            ConverterType = converterType;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentSchema"/> that represents a dynamic.
        /// </summary>
        protected ArgumentSchema(Type propertyType,
                                 string propertyName,
                                 string? name,
                                 string? description,
                                 Type? converterType,
                                 IMetadataCollection metadata)
        {
            Bindable = new BindableArgument(metadata, propertyType, propertyName);
            Name = name;
            Description = description;
            ConverterType = converterType;
        }
    }
}