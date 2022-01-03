namespace Typin.Features.Binding
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of unbounded directives.
    /// </summary>
    public interface IUnboundedDirectiveCollection : IList<IUnboundedDirectiveToken>
    {
        /// <summary>
        /// Whether collection has unbounded directives.
        /// </summary>
        bool HasUnbounded { get; }

        /// <summary>
        /// Whether all directives tokens were bounded.
        /// </summary>
        bool IsBounded { get; }
    }
}
