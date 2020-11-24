namespace TypinExamples.TypinWeb.Extensions
{
    using Microsoft.Extensions.Logging;
    using Typin;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;
    using TypinExamples.Infrastructure.TypinWeb.Logging;

    public static class CliApplicationBuilderExtensions
    {
        public static CliApplicationBuilder UseWebExample(this CliApplicationBuilder builder, WebCliConfiguration configuration)
        {
            if (configuration.Console is not null)
                builder.UseConsole(configuration.Console);

            if (configuration.LoggerDestination is not null)
            {
                builder.ConfigureLogging((cfg) =>
                {
                    cfg.ClearProviders();
                    cfg.AddWebLogger(configuration.LoggerDestination, (c) => c.LogLevel = LogLevel.Trace);
                    cfg.SetMinimumLevel(LogLevel.Trace);
                });
            }

            return builder;
        }
    }
}
