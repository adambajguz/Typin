namespace TypinExamples.Application.Handlers.Notifications.LogViewer
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
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
            private readonly ILoggerDestinationRepository _loggerDestinationRepository;
            private readonly ILogger _logger;

            public Handler(ILoggerDestinationRepository loggerDestinationRepository, ILogger<Handler> logger)
            {
                _loggerDestinationRepository = loggerDestinationRepository;
                _logger = logger;
            }

            public ValueTask HandleAsync(LogNotification request, IWorker worker, CancellationToken cancellationToken)
            {
                if (request.TerminalId is string id &&
                    request.Entry is LogEntry entry)
                {
                    if (_loggerDestinationRepository.GetOrDefault(id) is IWebLoggerDestination destination)
                    {
                        destination.WriteLog(entry);
                    }
                    else
                    {
                        _logger.LogError("Unknown logger destination for terminal {TerminalId}", id);
                    }
                }

                return default;
            }
        }
    }
}
