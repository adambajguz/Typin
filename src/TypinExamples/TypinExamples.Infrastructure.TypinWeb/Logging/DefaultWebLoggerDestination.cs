namespace TypinExamples.Infrastructure.TypinWeb.Logging
{
    using TypinExamples.Application.Handlers.Commands.Terminal;
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
            await _worker.CallCommandAsync(new LogCommand
            {
                TerminalId = _terminalId,
                Entry = entry
            });
        }
    }
}
