namespace Typin.Directives.Features
{
    using System;
    using System.Collections.Generic;
    using Typin.Directives;

    /// <summary>
    /// Directives feature.
    /// </summary>
    public interface IDirectivesFeature
    {
        /// <summary>
        /// Current command directives instances.
        /// This collection contains both standard and pipelined directives because a pipelined directive is also a standard directive.
        /// </summary>
        /// <exception cref="NullReferenceException">Throws when uninitialized.</exception>
        IReadOnlyList<IDirective> Instances { get; }

        /// <summary>
        /// Finds and returns first directive instance of given type or null when not found.
        /// </summary>
        IDirective? GetDirectiveInstance<T>()
            where T : IDirective;

        /// <summary>
        /// Finds and returns first directive instance of given type or null when not found.
        /// </summary>
        IDirective? GetDirectiveInstance(Type type);

        /// <summary>
        /// Finds and returns directive instances of given type or empty collection when not found.
        /// </summary>
        IEnumerable<IDirective> GetDirectiveInstances<T>()
            where T : IDirective;

        /// <summary>
        /// Finds and returns directive instances of given type or empty collection when not found.
        /// </summary>
        IEnumerable<IDirective> GetDirectiveInstances(Type type);
    }
}
