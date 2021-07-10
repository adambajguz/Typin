namespace Typin.Schemas
{
    using System;
    using System.Reflection;

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
                               Type? converter)
            : base(property, description, converter)
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
                               Type? converter)
            : base(propertyType, propertyName, true, description, converter)
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