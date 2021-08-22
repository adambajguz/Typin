namespace Typin.Components
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// CLI component provider.
    /// </summary>
    public interface ICliComponentProvider
    {
        /// <summary>
        /// Gets components by component type.
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        IReadOnlyList<Type> Get(Type componentType);

        /// <summary>
        /// Get components by base type.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        IReadOnlyList<Type> Get<TComponent>();
    }
}