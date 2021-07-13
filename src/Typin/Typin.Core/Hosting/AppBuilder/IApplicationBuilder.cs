namespace Typin.Hosting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a class that provides the mechanisms to configure an application's request pipeline.
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> that provides access to the application's service container.
        /// </summary>
        IServiceProvider ApplicationServices { get; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between middleware.
        /// </summary>
        IDictionary<string, object?> Properties { get; }

        /// <summary>
        /// Adds a middleware to the applicaiton's input processing pipeline.
        /// </summary>
        /// <param name="middlewareType"></param>
        /// <returns></returns>
        IApplicationBuilder Use(Type middlewareType);

        /// <summary>
        /// Adds a middleware to the applicaiton's input processing pipeline.
        /// </summary>
        /// <returns></returns>
        IApplicationBuilder Use<T>()
            where T : IMiddleware;

        /// <summary>
        /// Builds the delegate used by this application to process HTTP requests.
        /// </summary>
        void Build();
    }
}