namespace Typin.Console
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console.Internal;
    using Typin.Hosting;

    /// <summary>
    /// <see cref="ICliBuilder"/> console extensions.
    /// </summary>
    public static partial class CliBuilderExtensions
    {
        /// <summary>
        /// Adds a named console to the application.
        /// Console instance will not be automatically disposed.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="name"></param>
        /// <param name="console"></param>
        public static ICliBuilder AddConsole(this ICliBuilder cliBuilder,
                                             string name,
                                             IConsole console)
        {
            Type consoleType = console.GetType();

            cliBuilder.Services.AddSingleton(consoleType, console);
            cliBuilder.RegisterNamedConsole(name, consoleType, ServiceLifetime.Singleton);

            return cliBuilder;
        }

        /// <summary>
        /// Adds a named console to the application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="lifetime"></param>
        public static ICliBuilder AddConsole(this ICliBuilder cliBuilder,
                                             string name,
                                             Type type,
                                             ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            cliBuilder.Services.Add(ServiceDescriptor.Describe(type, type, lifetime));
            cliBuilder.RegisterNamedConsole(name, type, lifetime);

            return cliBuilder;
        }

        /// <summary>
        /// Adds a named console to the application.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cliBuilder"></param>
        /// <param name="name"></param>
        /// <param name="lifetime"></param>
        public static ICliBuilder AddConsole<T>(this ICliBuilder cliBuilder,
                                                string name,
                                                ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : class, IConsole
        {
            return cliBuilder.AddConsole(name, typeof(T), lifetime);
        }

        private static void RegisterNamedConsole(this ICliBuilder cliBuilder,
                                                 string name,
                                                 Type type,
                                                 ServiceLifetime lifetime)
        {
            _ = name ?? throw new ArgumentNullException(nameof(name));

            IServiceCollection services = cliBuilder.Services;

            services.Configure((Action<ConsoleProviderOptions>)(options =>
            {
                options.Consoles.Add(name, lifetime, type);
            }));

            if (!cliBuilder.SubsequentCall)
            {
                services.AddScoped<IConsoleProvider, ConsoleProvider>();

                if (name == string.Empty)
                {
                    services.AddScoped<IConsole>(provider =>
                    {
                        IConsoleProvider consoleProvider = provider.GetRequiredService<IConsoleProvider>();

                        return consoleProvider.Default ??
                            throw new ApplicationException("Default console was not registered for this application.");
                    });
                }
            }
        }
    }
}
