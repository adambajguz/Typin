namespace Typin.Plugins.Help
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PackSite.Library.Pipelining;
    using Typin.Hosting;

    /// <summary>
    /// Plugin registration extensions.
    /// </summary>
    public static class PluginRegistrationExtensions
    {
        /// <summary>
        /// Configures the application to use a default implementation of <see cref="IHelpWriter"/>.
        /// </summary>
        /// <param name="cliBuilder"></param>
        public static ICliBuilder AddHelp(this ICliBuilder cliBuilder)
        {
            cliBuilder.Services.Replace(ServiceDescriptor.Describe(typeof(IHelpWriter), typeof(DefaultHelpWriter), ServiceLifetime.Scoped));

            return cliBuilder;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IHelpWriter"/>.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="type"></param>
        /// <param name="lifetime"></param>
        public static ICliBuilder AddHelp(this ICliBuilder cliBuilder, Type type, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            cliBuilder.Services.Replace(ServiceDescriptor.Describe(typeof(IHelpWriter), type, lifetime));

            return cliBuilder;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IHelpWriter"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cliBuilder"></param>
        /// <param name="lifetime"></param>
        public static ICliBuilder AddHelp<T>(this ICliBuilder cliBuilder, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IHelpWriter
        {
            cliBuilder.Services.Replace(ServiceDescriptor.Describe(typeof(IHelpWriter), typeof(T), lifetime));

            return cliBuilder;
        }

        /// <summary>
        /// Adds support for help plugin.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IPipelineBuilder<CliContext> AddHelp(this IPipelineBuilder<CliContext> builder)
        {
            return builder.AddStep<HelpHandler>();
        }
    }
}
