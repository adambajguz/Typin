namespace TypinExamples.Helpers
{
    using Microsoft.Extensions.Logging;

    public static class HtmlHelpers
    {
        public static string GetLogLevelCSSClasses(LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => "text-ltrace",
                LogLevel.Debug => "text-ldbg",
                LogLevel.Information => "text-linfo",
                LogLevel.Warning => "text-lwrn",
                LogLevel.Error => "text-lerr",
                LogLevel.Critical => "text-lcrit",
                _ => string.Empty,
            };
        }

        public static string GetLogLevelAlias(LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => "trace",
                LogLevel.Debug => "debug",
                LogLevel.Information => "info",
                LogLevel.Warning => "warn",
                LogLevel.Error => "error",
                LogLevel.Critical => "critical",
                _ => level.ToString(),
            };
        }
    }
}
