namespace Typin.Internal.Schemas
{
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Schemas;

    /// <summary>
    /// Resolves an instance of <see cref="CommandParameterSchema"/>.
    /// </summary>
    internal static class CommandParameterSchemaResolver
    {
        /// <summary>
        /// Resolves <see cref="CommandParameterSchema"/>.
        /// </summary>
        public static CommandParameterSchema? TryResolve(PropertyInfo property)
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
    }
}