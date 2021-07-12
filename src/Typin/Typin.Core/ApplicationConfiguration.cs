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
        /// Mode types defined in this application.
        /// </summary>
        public IReadOnlyList<Type> ModeTypes { get; }

        /// <summary>
        /// Command types defined in this application.
        /// </summary>
        public IReadOnlyCollection<Type> CommandTypes { get; }

        /// <summary>
        /// Dynamic command types defined in this application.
        /// </summary>
        public IReadOnlyCollection<Type> DynamicCommandTypes { get; }

        /// <summary>
        /// Custom directives defined in this application.
        /// </summary>
        public IReadOnlyCollection<Type> DirectiveTypes { get; }

        /// <summary>
        /// Collection of middlewares in application.
        /// </summary>
        public IReadOnlyCollection<Type> MiddlewareTypes { get; }

        /// <summary>
        /// Startup mode type.
        /// </summary>
        public Type StartupMode { get; }

        /// <summary>
        /// Service collection.
        /// </summary>
        public IEnumerable<ServiceDescriptor> Services { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ApplicationConfiguration"/>.
        /// </summary>
        public ApplicationConfiguration(IReadOnlyList<Type> modeTypes,
                                        IReadOnlyCollection<Type> commandTypes,
                                        IReadOnlyCollection<Type> dynamicCommandTypes,
                                        IReadOnlyCollection<Type> customDirectives,
                                        LinkedList<Type> middlewareTypes,
                                        Type startupMode,
                                        IEnumerable<ServiceDescriptor> services)
        {
            ModeTypes = modeTypes;
            CommandTypes = commandTypes;
            DynamicCommandTypes = dynamicCommandTypes;
            DirectiveTypes = customDirectives;
            MiddlewareTypes = middlewareTypes;
            StartupMode = startupMode;
            Services = services;
        }
    }
}