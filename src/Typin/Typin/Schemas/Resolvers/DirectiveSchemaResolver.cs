namespace Typin.Schemas.Resolvers
{
    using System;
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Internal.Exceptions;

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
            if (!DirectiveSchema.IsDirectiveType(type))
                throw InternalTypinExceptions.InvalidDirectiveType(type);

            DirectiveAttribute attribute = type.GetCustomAttribute<DirectiveAttribute>()!;

            string name = attribute.Name.TrimStart('[').TrimEnd(']');
            if (string.IsNullOrWhiteSpace(name))
                throw InternalTypinExceptions.DirectiveNameIsInvalid(name, type);

            return new DirectiveSchema(
                type,
                name,
                attribute.Description,
                attribute.InteractiveModeOnly
            );
        }
    }
}