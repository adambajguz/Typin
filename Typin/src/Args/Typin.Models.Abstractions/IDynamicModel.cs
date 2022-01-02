namespace Typin.Models
{
    using System;
    using System.Linq;
    using Typin.Models.Collections;

    /// <summary>
    /// Represents a dynamic (expandable) model.
    /// </summary>
    public interface IDynamicModel : IModel
    {
        /// <summary>
        /// Dynamic arguments.
        /// </summary>
        IArgumentCollection Arguments { get; init; }

        /// <summary>
        /// Checks whether type is a valid dynamic model.
        /// </summary>
        public static new bool IsValidType(Type type)
        {
            return type.GetInterfaces()
                .Contains(typeof(IDynamicModel)) &&
                !type.IsAbstract &&
                !type.IsInterface;
        }
    }
}