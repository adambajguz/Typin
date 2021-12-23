namespace Typin.Models
{
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
    }
}