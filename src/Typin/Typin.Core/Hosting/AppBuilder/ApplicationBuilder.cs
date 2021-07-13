namespace Typin.Hosting.Builder
{
    using System;
    using System.Collections.Generic;
    using Typin.Hosting;
    using Typin.Schemas;

    /// <summary>
    /// Default implementation for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly LinkedList<Type> _middlewares = new();
        private readonly MiddlewarePipelineProvider _middlewarePipelineProvider;

        /// <inheritdoc/>
        public IServiceProvider ApplicationServices { get; }

        /// <inheritdoc/>
        public IDictionary<string, object?> Properties { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        /// <param name="middlewarePipelineProvider"></param>
        public ApplicationBuilder(IServiceProvider serviceProvider, MiddlewarePipelineProvider middlewarePipelineProvider)
        {
            Properties = new Dictionary<string, object?>(StringComparer.Ordinal);
            ApplicationServices = serviceProvider;
            _middlewarePipelineProvider = middlewarePipelineProvider;
        }

        private T? GetProperty<T>(string key)
        {
            return Properties.TryGetValue(key, out var value) ? (T?)value : default;
        }

        private void SetProperty<T>(string key, T value)
        {
            Properties[key] = value;
        }

        /// <inheritdoc/>
        public IApplicationBuilder Use(Type middlewareType)
        {
            if (!KnownTypesHelpers.IsMiddlewareType(middlewareType))
            {
                throw new ArgumentException($"Invalid middleware type '{middlewareType}'.", nameof(middlewareType));
            }

            _middlewares.AddLast(middlewareType);

            return this;
        }

        /// <inheritdoc/>
        public IApplicationBuilder Use<T>()
            where T : IMiddleware
        {
            _middlewares.AddLast(typeof(T));

            return this;
        }

        /// <inheritdoc/>
        public void Build()
        {
            _middlewarePipelineProvider.Middlewares = _middlewares;
        }
    }
}