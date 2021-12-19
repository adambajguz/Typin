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
            if (!KnownTypesHelpers.IsDirectiveType(type))
            {
                throw DirectiveResolverExceptions.InvalidDirectiveType(type);
            }

            DirectiveAttribute attribute = type.GetCustomAttribute<DirectiveAttribute>()!;

            if (modeTypes is not null)
            {
                if (attribute.SupportedModes is not null && attribute.SupportedModes.Except(modeTypes).Any())
                {
                    throw DirectiveResolverExceptions.InvalidSupportedModesInDirective(type, attribute);
                }

                if (attribute.ExcludedModes is not null && attribute.ExcludedModes.Except(modeTypes).Any())
                {
                    throw DirectiveResolverExceptions.InvalidExcludedModesInDirective(type, attribute);
                }
            }

            return new DirectiveSchema(
                type,
                attribute.Name,
                attribute.Description,
                attribute.SupportedModes,
                attribute.ExcludedModes
            );
        }
    }
}