namespace Typin.Features.Binding
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of unbounded directives.
    /// </summary>
    public interface IUnboundedDirectiveCollection : ICollection<IUnboundedDirectiveToken>
    {
        /// <summary>
        /// Whether collection has unbounded directives.
        /// </summary>
        bool HasUnbounded { get; }

        /// <summary>
        /// Whether all directives tokens were bounded.
        /// </summary>
        bool IsBounded { get; }

        /// <summary>
        /// Gets a given extension. Setting a null value removes the feature.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The requested extension, or null if it is not present.</returns>
        IUnboundedDirectiveToken? this[int id] { get; }

        /// <summary>
        /// Removes by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Remove(int id);
    }
}
