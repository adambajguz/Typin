namespace Typin
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Models;

    /// <summary>
    /// A command model.
    /// </summary>
    public interface ICommand : IModel
    {
        /// <summary>
        /// Checks whether type is a valid command.
        /// </summary>
        public static bool IsValidType(Type type)
        {
            return type.GetInterfaces().Contains(typeof(ICommand)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}