namespace Typin.Internal.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Internal.Exceptions;
    using Typin.Schemas;

    /// <summary>
    /// Resolves an instance of <see cref="DirectiveSchema"/>.
    /// </summary>
    internal static class DirectiveSchemaResolver
    {
        /// <summary>
        /// Resolves <see cref="DirectiveSchema"/>.
        /// </summary>
        public static DirectiveSchema Resolve(Type type, IReadOnlyList<Type> modeTypes)
        {
            if (!SchemasHelpers.IsDirectiveType(type))
                throw ResolversExceptions.InvalidDirectiveType(type);

            DirectiveAttribute attribute = type.GetCustomAttribute<DirectiveAttribute>()!;

            if (modeTypes != null && attribute.SupportedModes != null && attribute.SupportedModes.Except(modeTypes).Any())
                throw ResolversExceptions.InvalidSupportedModesInDirective(type);

            string name = attribute.Name.TrimStart('[').TrimEnd(']');
            if (string.IsNullOrWhiteSpace(name))
                throw ResolversExceptions.DirectiveNameIsInvalid(name, type);

            return new DirectiveSchema(
                type,
                name,
                attribute.Description,
                attribute.SupportedModes
            );
        }
    }
}