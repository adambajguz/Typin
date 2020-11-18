namespace TypinExamples.TypinWeb.Extensions
{
    using Microsoft.Extensions.Logging;
    using Typin;
    using TypinExamples.TypinWeb.Configuration;

    public static class CliApplicationBuilderExtensions
    {
        public static CliApplicationBuilder UseWebExample(this CliApplicationBuilder builder, WebCliConfiguration configuration)
        {
            if (configuration.Console is not null)
                builder.UseConsole(configuration.Console);

            if (configuration.LoggerProvider is not null)
            {
                builder.ConfigureLogging((cfg) =>
                {
                    if (configuration.LoggerProvider is not null)
                        cfg.AddProvider(configuration.LoggerProvider);
                });
            }

            return builder;
        }
    }
}
