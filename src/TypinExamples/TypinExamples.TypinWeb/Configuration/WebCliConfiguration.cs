namespace TypinExamples.TypinWeb.Configuration
{
    using Microsoft.Extensions.Logging;
    using Typin.Console;
    using TypinExamples.TypinWeb.Logging;

    public sealed class WebCliConfiguration
    {
        public IConsole? Console { get; set; }
        public IWebLoggerDestination? LoggerDestination { get; set; }
    }
}
