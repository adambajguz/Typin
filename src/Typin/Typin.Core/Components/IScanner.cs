namespace Typin.Components
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Represents a CLI component scanner.
    /// </summary>
    public interface IScanner
    {
        //TODO: add flag to switch whether to add services or not.

        /// <summary>
        /// A component type.
        /// </summary>
        Type ComponentType { get; }

        /// <summary>
        /// Added types to the scanner.
        /// </summary>
        IReadOnlyCollection<Type> Types { get; }

        /// <summary>
        /// Whether component is a valid component for scanner.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsValidComponent(Type type);

        /// <summary>
        /// Adds a component of specified type to the application.
        /// </summary>
        IScanner Single(Type type);

        /// <summary>
        /// Adds multiple components to the application.
        /// </summary>
        IScanner Multiple(IEnumerable<Type> types);

        /// <summary>
        /// Adds components from the specified assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        IScanner From(Assembly assembly);

        /// <summary>
        /// Adds components from the specified assemblies to the application.
        /// Only adds public valid types.
        /// </summary>
        IScanner From(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Adds components from the calling assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        IScanner FromThisAssembly();
    }

    /// <summary>
    /// Represents a CLI component.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public interface IScanner<TComponent> : IScanner
        where TComponent : notnull
    {
        /// <summary>
        /// Adds a component of specified type to the application.
        /// </summary>
        IScanner<TComponent> Single<T>()
            where T : class, TComponent;

        /// <summary>
        /// Adds a component of specified type to the application.
        /// </summary>
        new IScanner<TComponent> Single(Type type);

        /// <summary>
        /// Adds multiple components to the application.
        /// </summary>
        new IScanner<TComponent> Multiple(IEnumerable<Type> types);

        /// <summary>
        /// Adds components from the specified assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        new IScanner<TComponent> From(Assembly assembly);

        /// <summary>
        /// Adds components from the specified assemblies to the application.
        /// Only adds public valid types.
        /// </summary>
        new IScanner<TComponent> From(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Adds components from the calling assembly to the application.
        /// Only adds public valid types.
        /// </summary>
        new IScanner<TComponent> FromThisAssembly();
    }
}