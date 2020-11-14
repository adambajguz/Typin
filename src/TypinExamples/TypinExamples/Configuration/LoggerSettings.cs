namespace TypinExamples.Configuration
{
    public sealed class LoggerSettings
    {
        public bool IsBrowserOutputEnabled { get; init; }
        public string? BrowserOutputTemplate { get; init; }

        public bool IsConsoleOutputEnabled { get; init; }
        public string? ConsoleOutputTemplate { get; init; }

        public string? SentryDSN { get; init; }
        public bool SentryEnabled { get; init; }
    }
}
