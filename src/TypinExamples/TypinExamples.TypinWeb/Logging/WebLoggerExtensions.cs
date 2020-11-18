namespace TypinExamples.TypinWeb.Logging
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Web logger extension methods.
    /// </summary>
    public static class WebLoggerExtensions
    {
        /// <summary>
        /// Adds the file logger provider, aliased as 'Web', in the available services as singleton and binds the file logger options class to the 'Web' section of the appsettings.json file.
        /// </summary>
        public static ILoggingBuilder AddWebLogger(this ILoggingBuilder builder, IWebLoggerDestination webLoggerDestination)
        {
            builder.AddConfiguration();

            builder.Services.TryAddSingleton<ILoggerProvider>((serviceProvider) =>
            {
                IOptionsMonitor<WebLoggerOptions> options = serviceProvider.GetRequiredService<IOptionsMonitor<WebLoggerOptions>>();
                return new WebLoggerProvider(options, webLoggerDestination);
            });
            builder.Services.TryAddSingleton<IConfigureOptions<WebLoggerOptions>, WebLoggerOptionsSetup>();
            builder.Services.TryAddSingleton<IOptionsChangeTokenSource<WebLoggerOptions>, LoggerProviderOptionsChangeTokenSource<WebLoggerOptions, WebLoggerProvider>>();

            return builder;
        }

        /// <summary>
        /// Adds the file logger provider, aliased as 'Web', in the available services as singleton and binds the file logger options class to the 'File' section of the appsettings.json file.
        /// </summary>
        public static ILoggingBuilder AddWebLogger(this ILoggingBuilder builder, IWebLoggerDestination webLoggerDestination, Action<WebLoggerOptions> configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddWebLogger(webLoggerDestination);
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
