namespace Typin.Hosting
{
    using System;
    using Typin.Hosting.Builder;

    /// <summary>
    /// A factory for creating <see cref="IApplicationBuilder" /> instances.
    /// </summary>
    public class ApplicationBuilderFactory : IApplicationBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MiddlewarePipelineProvider _middlewarePipelineProvider;

        /// <summary>
        /// Initialize a new factory instance with an <see cref="IServiceProvider" />.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve dependencies and initialize components.</param>
        public ApplicationBuilderFactory(IServiceProvider serviceProvider, MiddlewarePipelineProvider middlewarePipelineProvider)
        {
            _serviceProvider = serviceProvider;
            _middlewarePipelineProvider = middlewarePipelineProvider;
        }

        /// <summary>
        /// Create an <see cref="IApplicationBuilder" /> builder given a <paramref name="serverFeatures" />.
        /// </summary>
        /// <returns>An <see cref="IApplicationBuilder"/> configured with <paramref name="serverFeatures"/>.</returns>
        public IApplicationBuilder CreateBuilder()
        {
            return new ApplicationBuilder(_serviceProvider, _middlewarePipelineProvider);
        }
    }
}