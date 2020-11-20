namespace TypinExamples.TypinWeb.Configuration
{
    using Typin.Console;
    using TypinExamples.TypinWeb.Logging;

    public sealed class WebCliConfiguration
    {
        public IConsole? Console { get; set; }
        public IWebLoggerDestination? LoggerDestination { get; set; }
    }
}
