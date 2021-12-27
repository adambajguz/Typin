namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.Hosting;

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
                using DefaultCliBuilder cliBuilder = new(context, services);
                configure(cliBuilder);
            });

            return builder;
        }
    }
}
