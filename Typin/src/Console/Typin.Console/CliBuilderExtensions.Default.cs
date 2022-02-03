namespace Typin.Console
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting;

    /// <summary>
    /// <see cref="ICliBuilder"/> console extensions.
    /// </summary>
    public static partial class CliBuilderExtensions
    {
        /// <summary>
        /// Adds a default console to the application.
        /// Console instance will not be automatically diposed.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="console"></param>
        public static ICliBuilder AddConsole(this ICliBuilder cliBuilder,
                                             IConsole console)
        {
            return cliBuilder.AddConsole(string.Empty, console);
        }

        /// <summary>
        /// Adds a default console to the application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="type"></param>
        /// <param name="lifetime"></param>
        public static ICliBuilder AddConsole(this ICliBuilder cliBuilder,
                                             Type type,
                                             ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            return cliBuilder.AddConsole(string.Empty, type, lifetime);
        }

        /// <summary>
        /// Adds a default console to the application.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cliBuilder"></param>
        /// <param name="lifetime"></param>
        public static ICliBuilder AddConsole<T>(this ICliBuilder cliBuilder,
                                                ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : class, IConsole
        {
            return cliBuilder.AddConsole(string.Empty, typeof(T), lifetime);
        }
    }
}
