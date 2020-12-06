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
            private readonly ILoggerDestinationRepository _loggerDestinationRepository;

            public Handler(ILoggerDestinationRepository loggerDestinationRepository)
            {
                _loggerDestinationRepository = loggerDestinationRepository;
            }

            public ValueTask HandleAsync(LogNotification request, IWorker worker, CancellationToken cancellationToken)
            {
                if (request.TerminalId is string id &&
                    request.Entry is LogEntry entry &&
                    _loggerDestinationRepository.GetOrDefault(id) is IWebLoggerDestination destination)
                {
                    destination.WriteLog(entry);
                }

                return default;
            }
        }
    }
}
