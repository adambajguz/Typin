namespace TypinExamples.TypinWeb.Logging.Base
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The information of a log entry.
    /// <para>The logger creates an instance of this class when its Log() method is called, fills the properties and then passes the info to the provider calling WriteLog(). </para>
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Date and time, in UTC, of the creation time of this instance
        /// </summary>
        public DateTime TimestampUtc { get; private set; }

        /// <summary>
        /// Category this instance belongs to.
        /// <para>The category is usually the fully qualified class name of a class asking for a logger, e.g. MyNamespace.MyClass </para>
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// The log level of this information.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// The message of this information
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// The exception this information represents, if any, else null.
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// The EventId of this information.
        /// <para>An EventId with Id set to zero, usually means no EventId.</para>
        /// </summary>
        public EventId EventId { get; set; }

        /// <summary>
        /// The state object. Contains information regarding the text message.
        /// <para>It looks like its type is always Microsoft.Extensions.Logging.Internal.FormattedLogValues </para>
        /// </summary>
        public object? State { get; set; }

        /// <summary>
        /// Used when State is just a string type. So far null.
        /// </summary>
        public string? StateText { get; set; }

        /// <summary>
        /// A dictionary with State properties.
        /// <para>When the log message is a message template with format values, e.g. <code>Logger.LogInformation("Customer {CustomerId} order {OrderId} is completed", CustomerId, OrderId)</code>  </para>
        /// this dictionary contains entries gathered from the message in order to ease any Structured Logging providers.
        /// </summary>
        public Dictionary<string, object>? StateProperties { get; set; }

        /// <summary>
        /// The scopes currently in use, if any. The last scope is
        /// </summary>
        public List<LogScopeInfo>? Scopes { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="LogEntry"/>.
        /// </summary>
        public LogEntry()
        {
            TimestampUtc = DateTime.UtcNow;
        }
    }
}