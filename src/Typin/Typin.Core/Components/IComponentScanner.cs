namespace Typin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Represents a CLI component scanner.
    /// </summary>
    public interface IComponentScanner
    {
        /// <summary>
        /// A component type.
        /// </summary>
        Type ComponentType { get; }

        /// <summary>
        /// Added types to the block.
        /// </summary>
        IReadOnlyList<Type> Types { get; }

        /// <summary>
        /// Whether component is a valid component for scanner.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsValidComponent(Type type);

        /// <summary>
        /// Adds a component of specified type to the application.
        /// </summary>
        IComponentScanner Single(Type type);

        /// <summary>
        /// Adds multiple components to the application.
        /// </summary>
        IComponentScanner Multiple(IEnumerable<Type> types);

        /// <summary>
        /// Adds components from the specified assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        IComponentScanner From(Assembly assembly);

        /// <summary>
        /// Adds components from the specified assemblies to the application.
        /// Only adds public valid types.
        /// </summary>
        IComponentScanner From(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Adds components from the calling assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        IComponentScanner FromThisAssembly();
    }

    /// <summary>
    /// Represents a CLI component.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public interface IComponentScanner<TComponent> : IComponentScanner
        where TComponent : notnull
    {
        /// <summary>
        /// Adds a component of specified type to the application.
        /// </summary>
        IComponentScanner<TComponent> Single<T>()
            where T : class, TComponent;

        /// <summary>
        /// Adds a component of specified type to the application.
        /// </summary>
        new IComponentScanner<TComponent> Single(Type type);

        /// <summary>
        /// Adds multiple components to the application.
        /// </summary>
        new IComponentScanner<TComponent> Multiple(IEnumerable<Type> types);

        /// <summary>
        /// Adds components from the specified assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        new IComponentScanner<TComponent> From(Assembly assembly);

        /// <summary>
        /// Adds components from the specified assemblies to the application.
        /// Only adds public valid types.
        /// </summary>
        new IComponentScanner<TComponent> From(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Adds components from the calling assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        new IComponentScanner<TComponent> FromThisAssembly();
    }
}