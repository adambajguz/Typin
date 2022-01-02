namespace Typin.Models.Schemas
{
    using System;
    using System.Reflection;
    using Typin.Models.Binding;
    using Typin.Schemas.Collections;

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

        /// <inheritdoc/>
        public IExtensionsCollection Extensions { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentSchema"/> that represents a property-based argument.
        /// </summary>
        protected ArgumentSchema(PropertyInfo property,
                                 string? name,
                                 string? description,
                                 Type? converterType,
                                 IExtensionsCollection extensions)
        {
            Bindable = new BindableArgument(this, property);
            Name = name;
            Description = description;
            ConverterType = converterType;
            Extensions = extensions;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ArgumentSchema"/> that represents a dynamic.
        /// </summary>
        protected ArgumentSchema(Type propertyType,
                                 string propertyName,
                                 string? name,
                                 string? description,
                                 Type? converterType,
                                 IExtensionsCollection extensions)
        {
            Bindable = new BindableArgument(this, propertyType, propertyName);
            Name = name;
            Description = description;
            ConverterType = converterType;
            Extensions = extensions;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Name)} = {Name}";
        }
    }
}