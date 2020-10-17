namespace Typin.Schemas
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Text;

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
        internal CommandParameterSchema(PropertyInfo? property, int order, string name, string? description)
            : base(property, description)
        {
            Order = order;
            Name = name;
        }

        internal string GetUserFacingDisplayString()
        {
            var buffer = new StringBuilder();

            buffer.Append('<')
                  .Append(Name)
                  .Append('>');

            return buffer.ToString();
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"{Property?.Name ?? "<implicit>"} ([{Order}] {GetUserFacingDisplayString()})";
        }
    }
}