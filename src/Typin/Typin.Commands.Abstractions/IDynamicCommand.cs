namespace Typin.Commands
{
    using System;
    using System.Linq;
    using Typin.Models;

    /// <summary>
    /// Represents a dynamic command.
    /// </summary>
    public interface IDynamicCommand : IDynamicModel
    {
        /// <summary>
        /// Checks whether type is a valid command.
        /// </summary>
        public static new bool IsValidType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IDynamicCommand)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}