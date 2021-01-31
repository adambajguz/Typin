namespace TypinExamples.Infrastructure.TypinWeb.Configuration
{
    using Typin.Console;
    using TypinExamples.Application.Services.TypinWeb;

    public sealed class WebCliConfiguration
    {
        public IConsole Console { get; }
        public IWebLoggerDestination? LoggerDestination { get; }

        public WebCliConfiguration(IConsole console, IWebLoggerDestination? loggerDestination = null)
        {
            Console = console;
            LoggerDestination = loggerDestination;
        }
    }
}
