namespace Typin.Hosting.Components
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// CLI component provider.
    /// </summary>
    internal struct Components<T> : IComponents<T>
    {
        /// <summary>
        /// Component types collection.
        /// </summary>
        public IReadOnlyCollection<Type>? Types { get; }

        /// <summary>
        /// Initializes a new instanse of <see cref="Components{T}"/>.
        /// </summary>
        /// <param name="componentProvider"></param>
        public Components(IComponentProvider componentProvider)
        {
            Types = componentProvider.Get<T>();
        }
    }
}