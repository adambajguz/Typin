namespace TypinExamples.Application.Handlers.Notifications.LogViewer
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Domain.Models.TypinLogging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public sealed class LogNotification : INotification
    {
        public string? TerminalId { get; init; }
        public LogEntry? Entry { get; init; }

        public class Handler : INotificationHandler<LogNotification>
        {
            private readonly ITerminalRepository _terminalRepository;

            public Handler(ITerminalRepository terminalRepository)
            {
                _terminalRepository = terminalRepository;
            }

            public async ValueTask HandleAsync(LogNotification request, IWorker worker, CancellationToken cancellationToken)
            {
                if (request.TerminalId is string id &&
                    _terminalRepository.GetOrDefault(id) is IWebTerminal webTerminal)
                {
                    await webTerminal.WriteLineAsync(request.Entry?.Text ?? string.Empty);
                }
            }
        }
    }
}
