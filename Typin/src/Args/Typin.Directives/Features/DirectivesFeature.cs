namespace Typin.Directives.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="IDirectivesFeature"/> implementation.
    /// </summary>
    internal sealed class DirectivesFeature : IDirectivesFeature
    {
        /// <inheritdoc/>
        public IReadOnlyList<DirectiveInstance> Instances { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectivesFeature"/>.
        /// </summary>
        public DirectivesFeature(IReadOnlyList<DirectiveInstance> instances)
        {
            Instances = instances;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Instances)} = {{{Instances}}}";
        }
    }
}
