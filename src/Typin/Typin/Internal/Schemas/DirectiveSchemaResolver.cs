//namespace Typin.Internal.Schemas
//{
//    using System;
//    using System.Linq;
//    using System.Reflection;
//    using Typin.Attributes;
//    using Typin.Directives.Schemas;
//    using Typin.Exceptions.Mode;
//    using Typin.Exceptions.Resolvers.DirectiveResolver;
//    using Typin.Internal.Exceptions;
//    using Typin.Schemas;

//    /// <summary>
//    /// Resolves an instance of <see cref="DirectiveSchema"/>.
//    /// </summary>
//    internal static class DirectiveSchemaResolver
//    {
//        /// <summary>
//        /// Resolves <see cref="DirectiveSchema"/>.
//        /// </summary>
//        public static IDirectiveSchema Resolve(Type type)
//        {
//            if (!IDirectiveSchema.IsDirectiveType(type))
//            {
//                throw new InvalidDirectiveException(type);
//            }

//            DirectiveAttribute attribute = type.GetCustomAttribute<DirectiveAttribute>()!;
//            string directiveName = attribute.Name.TrimStart('[').TrimEnd(']');

//            if (string.IsNullOrWhiteSpace(directiveName))
//            {
//                throw AttributesExceptions.DirectiveNameIsInvalid(directiveName);
//            }

//            if (attribute.SupportedModes is not null)
//            {
//                Type? invalidMode = attribute.SupportedModes.FirstOrDefault(x => !KnownTypesHelpers.IsCliModeType(x));

//                if (invalidMode is not null)
//                {
//                    throw new InvalidModeException(invalidMode);
//                }
//            }

//            if (attribute.ExcludedModes is not null)
//            {
//                Type? invalidMode = attribute.ExcludedModes.FirstOrDefault(x => !KnownTypesHelpers.IsCliModeType(x));

//                if (invalidMode is not null)
//                {
//                    throw new InvalidModeException(invalidMode);
//                }
//            }

//            return new DirectiveSchema(
//                type,
//                directiveName,
//                attribute.Description,
//                attribute.SupportedModes,
//                attribute.ExcludedModes
//            );
//        }
//    }
//}