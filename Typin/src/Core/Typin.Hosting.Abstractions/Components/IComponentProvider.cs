﻿namespace Typin.Hosting.Components
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// CLI component provider.
    /// </summary>
    public interface IComponentProvider
    {
        /// <summary>
        /// Component types collection.
        /// </summary>
        IReadOnlyCollection<Type> ComponentTypes { get; }

        /// <summary>
        /// Gets components.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested components, or null if it is not present.</returns>
        IReadOnlyCollection<Type>? this[Type key] { get; }

        /// <summary>
        /// Gets components by component type.
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        IReadOnlyCollection<Type>? Get(Type componentType);

        /// <summary>
        /// Get components by base type.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        IReadOnlyCollection<Type>? Get<TComponent>();
    }
}