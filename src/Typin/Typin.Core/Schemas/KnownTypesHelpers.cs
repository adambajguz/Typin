namespace Typin.Schemas
{
    using System;
    using System.Reflection;
    using Typin.Attributes;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Known types helpers.
    /// </summary>
    public static class KnownTypesHelpers
    {
        /// <summary>
        /// Checks whether type is a valid directive.
        /// </summary>
        public static bool IsDirectiveType(Type type)
        {
            return type.Implements(typeof(IDirective)) &&
                   type.IsDefined(typeof(DirectiveAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid command.
        /// </summary>
        public static bool IsCommandType(Type type)
        {
            return type.Implements(typeof(ICommand)) &&
                   type.IsDefined(typeof(CommandAttribute)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid CLI mode.
        /// </summary>
        public static bool IsCliModeType(Type type)
        {
            return type.Implements(typeof(ICliMode)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }

        /// <summary>
        /// Checks whether type is a valid command.
        /// </summary>
        public static bool IsDynamicCommandType(Type type)
        {
            return type.Implements(typeof(IDynamicCommand)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }
    }
}
