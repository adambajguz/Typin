namespace Typin.Hosting.Components
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// CLI component provider.
    /// </summary>
    public interface IComponents<T>
    {
        /// <summary>
        /// Component types collection.
        /// </summary>
        IReadOnlyCollection<Type>? Types { get; }
    }
}