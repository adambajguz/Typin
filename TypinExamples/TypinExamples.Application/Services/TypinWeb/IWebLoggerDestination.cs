namespace TypinExamples.Application.Services.TypinWeb
{
    using TypinExamples.Domain.Models.TypinLogging;

    public interface IWebLoggerDestination
    {
        void WriteLog(LogEntry entry);
    }
}
