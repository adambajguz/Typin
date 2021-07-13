namespace Typin.Hosting.Startup
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Provides an interface for initializing services and middleware used by an application.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Register services into the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="context">Host builder context.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        void ConfigureServices(HostBuilderContext context, IServiceCollection services);

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">An <see cref="IApplicationBuilder"/> for the app to configure.</param>
        void Configure(IApplicationBuilder app);
    }

    /// <summary>
    /// Provides an interface for initializing services and middleware used by an application.
    /// </summary>
    public interface IStartup<TBuilder> : IStartup
        where TBuilder : notnull
    {
        /// <summary>
        /// Sets up the service container.
        /// </summary>
        /// <param name="context">Host builder context.</param>
        /// <param name="builder">The builder associated with the container to configure.</param>
        void ConfigureContainer(HostBuilderContext context, TBuilder builder);
    }
}
