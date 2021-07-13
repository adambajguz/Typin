namespace Typin.Hosting.Components
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Represents a CLI component.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public interface ICliComponent<TComponent>
        where TComponent : notnull
    {
        /// <summary>
        /// Adds a component of specified type to the application.
        /// </summary>
        ICliComponent<TComponent> Single<T>()
            where T : class, TComponent;

        /// <summary>
        /// Adds a component of specified type to the application.
        /// </summary>
        ICliComponent<TComponent> Single(Type type);

        /// <summary>
        /// Adds multiple components to the application.
        /// </summary>
        ICliComponent<TComponent> Multiple(IEnumerable<Type> types);

        /// <summary>
        /// Adds components from the specified assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        ICliComponent<TComponent> From(Assembly assembly);

        /// <summary>
        /// Adds components from the specified assemblies to the application.
        /// Only adds public valid types.
        /// </summary>
        ICliComponent<TComponent> From(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Adds components from the calling assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        ICliComponent<TComponent> FromThisAssembly();
    }
}