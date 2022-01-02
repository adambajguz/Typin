namespace Typin
{
    using System;
    using System.Linq;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Pipeline middleware to surround the inner handler.
    /// Implementations add additional behavior and await the next delegate.
    /// </summary>
    public interface IMiddleware : IStep<CliContext>
    {
        /// <summary>
        /// Checks whether type is a valid middleware.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            return type.GetInterfaces()
                .Contains(typeof(IMiddleware)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}
