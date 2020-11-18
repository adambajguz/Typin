namespace TypinExamples.TypinWeb.Extensions
{
    using Typin;
    using TypinExamples.TypinWeb.Configuration;
    using TypinExamples.TypinWeb.Logging;

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
                    if (configuration.LoggerDestination is not null)
                    {
                        cfg.AddWebLogger(configuration.LoggerDestination);
                    }
                });
            }

            return builder;
        }
    }
}
