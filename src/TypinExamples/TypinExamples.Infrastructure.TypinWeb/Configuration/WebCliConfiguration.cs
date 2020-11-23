namespace TypinExamples.Infrastructure.TypinWeb.Configuration
{
    using Typin.Console;
    using TypinExamples.Application.Services.TypinWeb;

    public sealed class WebCliConfiguration
    {
        public IConsole? Console { get; set; }
        public IWebLoggerDestination? LoggerDestination { get; set; }
    }
}
