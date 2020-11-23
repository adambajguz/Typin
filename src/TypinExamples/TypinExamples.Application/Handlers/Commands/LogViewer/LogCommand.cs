namespace TypinExamples.Application.Handlers.Commands.Terminal
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Domain.Interfaces.Handlers.Core;

    public class LogCommand : ICoreRequest<Unit>
    {
        public long? WorkerId { get; set; }

        public string? TerminalId { get; init; }
        public string? Value { get; init; }

        public class LogHandler : ICoreRequestHandler<LogCommand>
        {
            private readonly ITerminalRepository _terminalRepository;

            public LogHandler(ITerminalRepository terminalRepository)
            {
                _terminalRepository = terminalRepository;
            }

            public async Task<Unit> Handle(LogCommand request, CancellationToken cancellationToken)
            {
                if (request.TerminalId is not null && request.Value is not null)
                {
                    IWebTerminal? webTerminal = _terminalRepository.GetOrDefault(request.TerminalId);

                    if (webTerminal is not null)
                        await webTerminal.WriteAsync(request.Value);
                }

                return Unit.Value;
            }
        }
    }
}
