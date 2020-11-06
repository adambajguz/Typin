namespace Typin.Schemas.Resolvers
{
    using System;
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Internal.Exceptions;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Resolves an instance of <see cref="DirectiveSchema"/>.
    /// </summary>
    internal static class DirectiveSchemaResolver
    {
        /// <summary>
        /// Resolves <see cref="DirectiveSchema"/>.
        /// </summary>
        public static DirectiveSchema Resolve(Type type)
        {
            if (!SchemasHelpers.IsDirectiveType(type))
                throw ResolversExceptions.InvalidDirectiveType(type);

            DirectiveAttribute attribute = type.GetCustomAttribute<DirectiveAttribute>()!;

            string name = attribute.Name.TrimStart('[').TrimEnd(']');
            if (string.IsNullOrWhiteSpace(name))
                throw ResolversExceptions.DirectiveNameIsInvalid(name, type);

            return new DirectiveSchema(
                type,
                name,
                attribute.Description,
                attribute.InteractiveModeOnly
            );
        }
    }
}