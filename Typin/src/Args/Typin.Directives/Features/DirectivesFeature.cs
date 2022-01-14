namespace Typin.Directives.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="IDirectivesFeature"/> implementation.
    /// </summary>
    internal sealed class DirectivesFeature : IDirectivesFeature
    {
        /// <inheritdoc/>
        public IReadOnlyList<IDirective> Instances { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectivesFeature"/>.
        /// </summary>
        public DirectivesFeature(IReadOnlyList<IDirective> instances)
        {
            Instances = instances;
        }
    }
}
