namespace Typin.Hosting
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.EventLog;

    /// <summary>
    /// Typin command line host builder extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds and configures a Typin command line application.
        /// </summary>
        public static IHostBuilder ConfigureCliHost(this IHostBuilder builder, Action<ICliBuilder> configure)
        {
            _ = configure ?? throw new ArgumentNullException(nameof(configure));

            builder.ConfigureServices((context, services) =>
            {
                using CliBuilder cliBuilder = new(context, services);
                configure(cliBuilder);
            });

            return builder;
        }

        /// <summary>
        /// Configures an existing <see cref="IHostBuilder"/> instance with pre-configured defaults. This will overwrite
        /// previously configured values and is intended to be called before additional configuration calls.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the <see cref="IHostBuilder"/>:
        ///   <list type="bullet">
        ///     <item><description>set the <see cref="IHostEnvironment.ContentRootPath"/> to the result of <see cref="Directory.GetCurrentDirectory()"/></description></item>
        ///     <item><description>load host <see cref="IConfiguration"/> from "DOTNET_" prefixed environment variables</description></item>
        ///     <item><description>load app <see cref="IConfiguration"/> from 'appsettings.json' and 'appsettings.[<see cref="IHostEnvironment.EnvironmentName"/>].json'</description></item>
        ///     <item><description>load app <see cref="IConfiguration"/> from User Secrets when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development' using the entry assembly</description></item>
        ///     <item><description>load app <see cref="IConfiguration"/> from environment variables</description></item>
        ///     <item><description>configure the <see cref="ILoggerFactory"/> to log to the debug and event source output</description></item>
        ///     <item><description>enables scope validation on the dependency injection container when <see cref="IHostEnvironment.EnvironmentName"/> is 'Development'</description></item>
        ///   </list>
        /// </remarks>
        /// <param name="builder">The existing builder to configure.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder ConfigureCliDefaults(this IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddEnvironmentVariables(prefix: "DOTNET_");
            });

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                IHostEnvironment env = hostingContext.HostingEnvironment;
                bool reloadOnChange = GetReloadConfigOnChangeValue(hostingContext);

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: reloadOnChange);

                if (env.IsDevelopment() && env.ApplicationName is { Length: > 0 })
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly is not null)
                    {
                        config.AddUserSecrets(appAssembly, optional: true, reloadOnChange: reloadOnChange);
                    }
                }

                config.AddEnvironmentVariables();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                // IMPORTANT: This needs to be added *before* configuration is loaded, this lets
                // the defaults be overridden by the configuration.
                if (isWindows)
                {
                    // Default the EventLogLoggerProvider to warning or above
                    logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Warning);
                }

                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();

                if (isWindows)
                {
                    // Add the EventLogLoggerProvider on windows machines
                    logging.AddEventLog();
                }

#if NET5_0_OR_GREATER
                logging.Configure(options =>
                {
                    options.ActivityTrackingOptions =
                        ActivityTrackingOptions.SpanId |
                        ActivityTrackingOptions.TraceId |
                        ActivityTrackingOptions.ParentId;
                });
#endif
            })
            .UseDefaultServiceProvider((context, options) =>
            {
                bool isDevelopment = context.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            });

            return builder;

            static bool GetReloadConfigOnChangeValue(HostBuilderContext hostingContext)
            {
                return hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", defaultValue: true);
            }
        }
    }
}
