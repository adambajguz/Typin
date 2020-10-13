namespace Typin.Schemas
{
    using System.Reflection;
    using Typin.Attributes;

    /// <summary>
    /// Resolves an instance of <see cref="CommandOptionSchema"/>.
    /// </summary>
    internal static class CommandOptionSchemaResolver
    {
        /// <summary>
        /// Resolves <see cref="CommandOptionSchema"/>.
        /// </summary>
        internal static CommandOptionSchema? TryResolve(PropertyInfo property)
        {
            CommandOptionAttribute? attribute = property.GetCustomAttribute<CommandOptionAttribute>();
            if (attribute is null)
                return null;

            // The user may mistakenly specify dashes, thinking it's required, so trim them
            string? name = attribute.Name?.TrimStart('-');

            return new CommandOptionSchema(
                property,
                name,
                attribute.ShortName,
                attribute.FallbackVariableName,
                attribute.IsRequired,
                attribute.Description
            );
        }
    }
}