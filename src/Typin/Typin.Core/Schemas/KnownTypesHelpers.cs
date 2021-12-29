namespace Typin.Schemas
{
    using System;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Known types helpers.
    /// </summary>
    public static class KnownTypesHelpers
    {
        /// <summary>
        /// Checks whether type is a valid middleware.
        /// </summary>
        public static bool IsMiddlewareType(Type type)
        {
            return type.Implements(typeof(IMiddleware)) &&
                   !type.IsAbstract &&
                   !type.IsInterface;
        }
    }
}
