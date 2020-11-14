namespace Typin.Internal.Schemas
{
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

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
            char? shortName = attribute.ShortName == '-' ? null : attribute.ShortName;

            if (shortName is null && string.IsNullOrWhiteSpace(name))
            {
                string propertyName = property.Name;
                name = propertyName.ToHyphenCase();
            }

            return new CommandOptionSchema(
                property,
                name,
                shortName,
                attribute.FallbackVariableName,
                attribute.IsRequired,
                attribute.Description
            );
        }
    }
}