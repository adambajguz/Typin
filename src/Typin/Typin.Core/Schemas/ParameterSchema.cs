namespace Typin.Schemas
{
    using System;
    using System.Reflection;
    using Typin.Metadata;

    /// <summary>
    /// Stores command parameter schema.
    /// </summary>
    public class ParameterSchema : ArgumentSchema
    {
        /// <summary>
        /// Parameter order.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Parameter name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterSchema"/> that represents a property-based parameter.
        /// </summary>
        public ParameterSchema(PropertyInfo property,
                               int order,
                               string name,
                               string? description,
                               Type? converter,
                               IMetadataCollection metadata)
            : base(property, description, converter, metadata)
        {
            Order = order;
            Name = name;
        }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterSchema"/> that represents a dynamic parameter.
        /// </summary>
        public ParameterSchema(Type propertyType,
                               string propertyName,
                               int order,
                               string name,
                               string? description,
                               Type? converter,
                               IMetadataCollection metadata)
            : base(propertyType, propertyName, true, description, converter, metadata)
        {
            Order = order;
            Name = name;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Bindable.Name} ([{Order}] <{Name}>)";
        }
    }
}