namespace TypinExamples.Infrastructure.TypinWeb.Logging.Base
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Logging;
    using TypinExamples.Domain.Models.TypinLogging;

    /// <summary>
    /// A base logger provider class.
    /// <para>A logger provider essentialy represents the medium where log information is saved.</para>
    /// <para>This class may serve as base class in writing a file or database logger provider.</para>
    /// </summary>
    public abstract class LoggerProvider : IDisposable, ILoggerProvider, ISupportExternalScope
    {
        private readonly ConcurrentDictionary<string, Logger> loggers = new ConcurrentDictionary<string, Logger>();
        private IExternalScopeProvider? fScopeProvider;
        protected IDisposable? SettingsChangeToken;

        /// <summary>
        /// Returns true when this instance is disposed.
        /// </summary>
        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// Initializes an instance of <see cref="LoggerProvider"/>.
        /// </summary>
        public LoggerProvider()
        {

        }

        /// <summary>
        /// Finalizes an instance of <see cref="LoggerProvider"/>.
        /// </summary>
        ~LoggerProvider()
        {
            if (!IsDisposed)
                Dispose(false);
        }

        /// <summary>
        /// Called by the logging framework in order to set external scope information source for the logger provider.
        /// <para>ISupportExternalScope implementation</para>
        /// </summary>
        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            fScopeProvider = scopeProvider;
        }

        /// <summary>
        /// Returns an ILogger instance to serve a specified category.
        /// <para>The category is usually the fully qualified class name of a class asking for a logger, e.g. MyNamespace.MyClass </para>
        /// </summary>
        ILogger ILoggerProvider.CreateLogger(string category)
        {
            return loggers.GetOrAdd(category,
            (category) =>
            {
                return new Logger(this, category);
            });
        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        void IDisposable.Dispose()
        {
            if (!IsDisposed)
            {
                try
                {
                    Dispose(true);
                }
                catch
                {

                }

                IsDisposed = true;
                GC.SuppressFinalize(this);  // instructs GC not bother to call the destructor
            }
        }

        /// <summary>
        /// Disposes the options change toker. IDisposable pattern implementation.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (SettingsChangeToken != null)
            {
                SettingsChangeToken.Dispose();
                SettingsChangeToken = null;
            }
        }

        /// <summary>
        /// Returns true if a specified log level is enabled.
        /// <para>Called by logger instances created by this provider.</para>
        /// </summary>
        public abstract bool IsEnabled(LogLevel logLevel);

        /// <summary>
        /// The loggers do not actually log the information in any medium.
        /// Instead the call their provider WriteLog() method, passing the gathered log information.
        /// </summary>
        public abstract void WriteLog(LogEntry entry);

        /// <summary>
        /// Returns the scope provider.
        /// <para>Called by logger instances created by this provider.</para>
        /// </summary>
        internal IExternalScopeProvider ScopeProvider
        {
            get
            {
                if (fScopeProvider is null)
                    fScopeProvider = new LoggerExternalScopeProvider();

                return fScopeProvider;
            }
        }
    }
}