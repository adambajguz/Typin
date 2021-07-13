namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.Hosting;
    using Typin.Hosting.Startup;

    /// <summary>
    /// Typin command line host builder.
    /// </summary>
    public interface ICliHostBuilder
    {
        /// <summary>
        /// Exposes access to the underlying host builder instance.
        /// </summary>
        public IHostBuilder HostBuilder { get; }

        /// <summary>
        /// Configures command line application.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        ICliHostBuilder Configure(Action<IApplicationBuilder> configure);

        /// <summary>
        /// Configures command line application using a startup class.
        /// </summary>
        /// <returns></returns>
        ICliHostBuilder UseStartup<TStartup>()
            where TStartup : notnull, IStartup, new();

        /// <summary>
        /// Configures command line application with a custom dependency injection container using a startup class.
        /// </summary>
        /// <returns></returns>
        ICliHostBuilder UseStartup<TStartup, TContainerBuilder>()
            where TStartup : IStartup<TContainerBuilder>, new()
            where TContainerBuilder : notnull;
    }
}