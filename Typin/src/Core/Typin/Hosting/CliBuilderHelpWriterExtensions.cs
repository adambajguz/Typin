namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Typin.Plugins.Help;

    /// <summary>
    /// <see cref="ICliBuilder"/> help writer extensions.
    /// </summary>
    public static class CliBuilderHelpWriterExtensions
    {
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IHelpWriter"/>.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="type"></param>
        /// <param name="lifetime"></param>
        public static ICliBuilder UseHelpWriter(this ICliBuilder cliBuilder, Type type, ServiceLifetime lifetime = ServiceLifetime.Scoped)
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
        public static ICliBuilder UseHelpWriter<T>(this ICliBuilder cliBuilder, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IHelpWriter
        {
            cliBuilder.Services.Replace(ServiceDescriptor.Describe(typeof(IHelpWriter), typeof(T), lifetime));

            return cliBuilder;
        }
    }
}
