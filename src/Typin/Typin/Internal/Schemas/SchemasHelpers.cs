namespace Typin.Internal.Schemas
{
    using System;
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Internal.Extensions;

    internal static class SchemasHelpers
    {
        public static bool IsDirectiveType(Type type)
        {
            return type.Implements(typeof(IDirective)) &&
                   type.IsDefined(typeof(DirectiveAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        public static bool IsCommandType(Type type)
        {
            return type.Implements(typeof(ICommand)) &&
                   type.IsDefined(typeof(CommandAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        public static bool IsCliModeType(Type type)
        {
            return type.Implements(typeof(ICliMode)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }
    }
}
