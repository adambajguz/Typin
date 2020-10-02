namespace Typin.Schemas
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Text;
    using Typin.Attributes;

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

        #region ctor
        /// <summary>
        /// Initializes an instance of <see cref="CommandParameterSchema"/>.
        /// </summary>
        private CommandParameterSchema(PropertyInfo? property, int order, string name, string? description)
            : base(property, description)
        {
            Order = order;
            Name = name;
        }

        /// <summary>
        /// Resolves <see cref="CommandParameterSchema"/>.
        /// </summary>
        internal static CommandParameterSchema? TryResolve(PropertyInfo property)
        {
            CommandParameterAttribute? attribute = property.GetCustomAttribute<CommandParameterAttribute>();
            if (attribute is null)
                return null;

            string name = attribute.Name ?? property.Name.ToLowerInvariant();

            return new CommandParameterSchema(
                property,
                attribute.Order,
                name,
                attribute.Description
            );
        }
        #endregion

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