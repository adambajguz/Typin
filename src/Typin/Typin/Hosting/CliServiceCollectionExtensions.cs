namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting.Internal;

    /// <summary>
    /// Cli service collection extensions.
    /// </summary>
    public static class CliServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Typin command line.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cli"></param>
        /// <returns></returns>
        public static IServiceCollection AddCli(this IServiceCollection services, Action<ICliBuilder> cli)
        {
            CliBuilder components = new(services);
            cli(components);

            return services;
        }
    }
}
