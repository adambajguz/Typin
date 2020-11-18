namespace TypinExamples.TypinWeb.Logging
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TypinExamples.TypinWeb.Logging.Base;

    /// <summary>
    /// A logger provider that writes log entries to a text file.
    /// <para>"Web" is the provider alias of this provider and can be used in the Logging section of the appsettings.json.</para>
    /// </summary>
    [ProviderAlias("Web")]
    public class WebLoggerProvider : LoggerProvider
    {
        private readonly List<LogEntry> _infoQueue = new();
        private readonly IWebLoggerDestination _webLoggerDestination;
        private WebLoggerOptions _settings;

        /// <summary>
        /// Constructor.
        /// <para>The IOptionsMonitor provides the OnChange() method which is called when the user alters the settings of this provider in the appsettings.json file.</para>
        /// </summary>
        public WebLoggerProvider(IOptionsMonitor<WebLoggerOptions> settings, IWebLoggerDestination webLoggerDestination)
            : this(settings.CurrentValue, webLoggerDestination)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/change-tokens
            SettingsChangeToken = settings.OnChange(settings =>
            {
                _settings = settings;
            });
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebLoggerProvider(WebLoggerOptions settings, IWebLoggerDestination webLoggerDestination)
        {
            _webLoggerDestination = webLoggerDestination;
            _settings = settings;
        }

        /// <summary>
        /// Checks if the given logLevel is enabled. It is called by the Logger.
        /// </summary>
        public override bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None &&
                   _settings.LogLevel != LogLevel.None &&
                   Convert.ToInt32(logLevel) >= Convert.ToInt32(_settings.LogLevel);
        }

        public override void WriteLog(LogEntry entry)
        {
            _webLoggerDestination.WriteLog(entry);
        }

        ///// <summary>
        ///// Pops a log info instance from the stack, prepares the text line, and writes the line to the text file.
        ///// </summary>
        //private void WriteLogLine()
        //{
        //    if (_infoQueue.TryDequeue(out LogEntry Info))
        //    {
        //        string S;
        //        StringBuilder SB = new StringBuilder();
        //        SB.Append(Pad(Info.TimeStampUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.ff"), _lengths["Time"]));
        //        SB.Append(Pad(Info.Level.ToString(), _lengths["Level"]));
        //        SB.Append(Pad(Info.EventId != null ? Info.EventId.ToString() : "", _lengths["EventId"]));
        //        SB.Append(Pad(Info.Category, _lengths["Category"]));

        //        S = "";
        //        if (Info.Scopes != null && Info.Scopes.Count > 0)
        //        {
        //            LogScopeInfo SI = Info.Scopes.Last();
        //            if (!string.IsNullOrWhiteSpace(SI.Text))
        //                S = SI.Text;
        //            else
        //            {
        //            }
        //        }

        //        string? text = Info.Text;

        //        /* writing properties is too much for a text file logger
        //        if (Info.StateProperties != null && Info.StateProperties.Count > 0)
        //        {
        //            Text = Text + " Properties = " + Newtonsoft.Json.JsonConvert.SerializeObject(Info.StateProperties);
        //        }
        //         */

        //        if (!string.IsNullOrWhiteSpace(text))
        //            SB.Append(text.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "));

        //        SB.AppendLine();
        //    }
        //}

        /// <summary>
        /// Disposes the options change toker. IDisposable pattern implementation.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}