namespace TypinExamples.TypinWeb.Logging
{
    using TypinExamples.TypinWeb.Logging.Base;

    public interface IWebLoggerDestination
    {
        void WriteLog(LogEntry entry);
    }
}
