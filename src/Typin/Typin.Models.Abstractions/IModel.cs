namespace Typin.Models
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a bindable model.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Checks whether type is a valid command.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(IModel)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}