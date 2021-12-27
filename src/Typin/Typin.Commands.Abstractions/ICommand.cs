namespace Typin.Commands
{
    using System;
    using System.Linq;
    using Typin.Models;

    /// <summary>
    /// A command.
    /// </summary>
    public interface ICommand : IModel
    {
        /// <summary>
        /// Checks whether type is a valid command.
        /// </summary>
        public static new bool IsValidType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(ICommand)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}