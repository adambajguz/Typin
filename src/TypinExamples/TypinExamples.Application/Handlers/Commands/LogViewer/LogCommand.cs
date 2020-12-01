namespace TypinExamples.Application.Handlers.Commands.Terminal
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.TypinWeb;

    public class LogCommand
    {
        public string? TerminalId { get; init; }
        public string? Value { get; init; }

        public class LogHandler
        {
            private readonly ITerminalRepository _terminalRepository;

            public LogHandler(ITerminalRepository terminalRepository)
            {
                _terminalRepository = terminalRepository;
            }

            public async Task Handle(LogCommand request, CancellationToken cancellationToken)
            {
                if (request.TerminalId is not null && request.Value is not null)
                {
                    IWebTerminal? webTerminal = _terminalRepository.GetOrDefault(request.TerminalId);

                    if (webTerminal is not null)
                        await webTerminal.WriteAsync(request.Value);
                }
            }
        }
    }
}
