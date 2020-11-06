namespace Typin
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Configuration of the application.
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary>
        /// Command types defined in this application.
        /// </summary>
        public IReadOnlyList<Type> CommandTypes { get; }

        /// <summary>
        /// Custom directives defined in this application.
        /// </summary>
        public IReadOnlyList<Type> DirectiveTypes { get; }

        /// <summary>
        /// Collection of middlewares in application.
        /// </summary>
        public IReadOnlyCollection<Type> Middlewares => MiddlewareTypes;

        /// <summary>
        /// Collection of middlewares in application.
        /// </summary>
        internal LinkedList<Type> MiddlewareTypes { get; }

        /// <summary>
        /// Service collection.
        /// </summary>
        public IEnumerable<ServiceDescriptor> Services { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ApplicationConfiguration"/>.
        /// </summary>
        public ApplicationConfiguration(IReadOnlyList<Type> commandTypes,
                                        IReadOnlyList<Type> customDirectives,
                                        LinkedList<Type> middlewareTypes,
                                        IEnumerable<ServiceDescriptor> services)
        {
            CommandTypes = commandTypes;
            DirectiveTypes = customDirectives;
            MiddlewareTypes = middlewareTypes;
            Services = services;
        }
    }
}