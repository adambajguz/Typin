namespace TypinExamples.Configuration
{
    public sealed class LoggerSettings
    {
        public bool IsBrowserOutputEnabled { get; set; }
        public string? BrowserOutputTemplate { get; set; }

        public bool IsConsoleOutputEnabled { get; set; }
        public string? ConsoleOutputTemplate { get; set; }

        public string? SentryDSN { get; set; }
        public bool SentryEnabled { get; set; }
    }
}
