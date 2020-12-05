namespace TypinExamples.Infrastructure.TypinWeb.Logging
{
    using TypinExamples.Application.Handlers.Notifications.LogViewer;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Domain.Models.TypinLogging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    internal sealed class DefaultWebLoggerDestination : IWebLoggerDestination
    {
        private readonly IWorker _worker;
        private readonly string _terminalId;

        public DefaultWebLoggerDestination(IWorker worker, string terminalId)
        {
            _worker = worker;
            _terminalId = terminalId;
        }

        public async void WriteLog(LogEntry entry)
        {
            await _worker.NotifyAsync(new LogNotification
            {
                TerminalId = _terminalId,
                Entry = entry
            });
        }
    }
}
