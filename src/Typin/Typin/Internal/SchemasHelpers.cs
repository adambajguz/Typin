namespace Typin.Internal.Extensions
{
    using System;
    using System.Reflection;
    using Typin.Attributes;

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
    }
}
