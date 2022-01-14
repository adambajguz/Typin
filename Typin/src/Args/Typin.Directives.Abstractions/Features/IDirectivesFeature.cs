namespace Typin.Directives.Features
{
    using System.Collections.Generic;
    using Typin.Directives;

    /// <summary>
    /// Directives feature.
    /// </summary>
    public interface IDirectivesFeature
    {
        /// <summary>
        /// Current directives instances.
        /// </summary>
        IReadOnlyList<IDirective> Instances { get; }
    }
}
