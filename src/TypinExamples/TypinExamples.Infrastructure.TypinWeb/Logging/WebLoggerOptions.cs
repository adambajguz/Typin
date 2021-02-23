namespace TypinExamples.Infrastructure.TypinWeb.Logging
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Options for the file logger.
    /// <para>There are two ways to configure file logger: 1. using the ConfigureLogging() in Program.cs or using the appsettings.json file.</para>
    /// </summary>
    public class WebLoggerOptions
    {
        /// <summary>
        /// The active log level. Defaults to LogLevel.Information
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Initializes an instance of <see cref="LogScopeInfo"/>.
        /// </summary>
        public WebLoggerOptions()
        {

        }
    }
}