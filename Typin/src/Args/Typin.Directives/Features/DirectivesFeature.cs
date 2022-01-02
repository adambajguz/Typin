namespace Typin.Directives.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        /// <summary>
        /// Finds and returns first directive instance of given type or null when not found.
        /// </summary>
        public IDirective? GetDirectiveInstance<T>()
                  where T : IDirective
        {
            return GetDirectiveInstance(typeof(T));
        }

        /// <summary>
        /// Finds and returns first directive instance of given type or null when not found.
        /// </summary>
        public IDirective? GetDirectiveInstance(Type type)
        {
            return Instances?.Where(x => x.GetType() == type).FirstOrDefault();
        }

        /// <summary>
        /// Finds and returns directive instances of given type or empty collection when not found.
        /// </summary>
        public IEnumerable<IDirective> GetDirectiveInstances<T>()
            where T : IDirective
        {
            return GetDirectiveInstances(typeof(T));
        }

        /// <summary>
        /// Finds and returns directive instances of given type or empty collection when not found.
        /// </summary>
        public IEnumerable<IDirective> GetDirectiveInstances(Type type)
        {
            return Instances?.Where(x => x.GetType() == type) ?? Enumerable.Empty<IDirective>();
        }
    }
}
