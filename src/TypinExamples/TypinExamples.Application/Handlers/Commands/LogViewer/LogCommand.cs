namespace TypinExamples.Application.Handlers.Commands.Terminal
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Domain.Models.TypinLogging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;

    public sealed class LogCommand : ICommand
    {
        public string? TerminalId { get; init; }
        public LogEntry? Entry { get; init; }

        public class Handler : ICommandHandler<LogCommand>
        {
            private readonly ITerminalRepository _terminalRepository;

            public Handler(ITerminalRepository terminalRepository)
            {
                _terminalRepository = terminalRepository;
            }

            public async ValueTask<CommandFinished> HandleAsync(LogCommand request, IWorker worker, CancellationToken cancellationToken)
            {
                if (request.TerminalId is string id &&
                    _terminalRepository.GetOrDefault(id) is IWebTerminal webTerminal)
                {
                    await webTerminal.WriteLineAsync(request.Entry?.Text ?? string.Empty);
                }

                return CommandFinished.Instance;
            }
        }
    }
}
