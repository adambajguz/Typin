namespace Typin.Directives.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// Directives feature.
    /// </summary>
    public interface IDirectivesFeature
    {
        /// <summary>
        /// Current directives instances.
        /// </summary>
        IReadOnlyList<DirectiveInstance> Instances { get; }
    }
}
