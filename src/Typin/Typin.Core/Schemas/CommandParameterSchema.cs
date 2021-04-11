namespace Typin.Schemas
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Stores command parameter schema.
    /// </summary>
    public class CommandParameterSchema : ArgumentSchema
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
        /// Initializes an instance of <see cref="CommandParameterSchema"/>.
        /// </summary>
        public CommandParameterSchema(PropertyInfo? property,
                                      int order,
                                      string name,
                                      string? description,
                                      Type? converter)
            : base(property, description, converter)
        {
            Order = order;
            Name = name;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Property?.Name ?? "<implicit>"} ([{Order}] <{Name}>)";
        }
    }
}