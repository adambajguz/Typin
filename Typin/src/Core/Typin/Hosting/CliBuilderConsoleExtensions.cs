namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Typin.Console;

    /// <summary>
    /// <see cref="ICliBuilder"/> console extensions.
    /// </summary>
    public static class CliBuilderConsoleExtensions
    {
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// Console instance will not be automatically diposed.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="console"></param>
        public static ICliBuilder UseConsole(this ICliBuilder cliBuilder, IConsole console)
        {
            cliBuilder.Services.Replace(ServiceDescriptor.Singleton<IConsole>(console));

            return cliBuilder;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="type"></param>
        public static ICliBuilder UseConsole(this ICliBuilder cliBuilder, Type type)
        {
            cliBuilder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConsole), type));

            return cliBuilder;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cliBuilder"></param>
        public static ICliBuilder UseConsole<T>(this ICliBuilder cliBuilder)
            where T : class, IConsole
        {
            cliBuilder.Services.Replace(ServiceDescriptor.Singleton<IConsole, T>());

            return cliBuilder;
        }
    }
}
