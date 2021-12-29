namespace Typin.Directives
{
    using System;
    using System.Linq;
    using Typin.Models;

    /// <summary>
    /// A directive.
    /// </summary>
    public interface IDirective : IModel
    {
        /// <summary>
        /// Checks whether type is a valid command.
        /// </summary>
        public static new bool IsValidType(Type type)
        {
            return type.GetInterfaces()
                .Contains(typeof(IDirective)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}