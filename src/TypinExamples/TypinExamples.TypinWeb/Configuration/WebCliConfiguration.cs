namespace TypinExamples.TypinWeb.Configuration
{
    using Microsoft.Extensions.Logging;
    using Typin.Console;

    public sealed class WebCliConfiguration
    {
        public IConsole? Console { get; set; }
        public ILoggerProvider? LoggerProvider { get; set; }
    }
}
