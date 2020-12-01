namespace TypinExamples.Application.Handlers.Commands.Terminal
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.TypinWeb;

    public class ClearCommand
    {
        public string? TerminalId { get; init; }
        public string? Value { get; init; }

        public class ClearHandler
        {
            private readonly ITerminalRepository _terminalRepository;

            public ClearHandler(ITerminalRepository terminalRepository)
            {
                _terminalRepository = terminalRepository;
            }

            public async Task Handle(ClearCommand request, CancellationToken cancellationToken)
            {
                if (request.TerminalId is not null && request.Value is not null)
                {
                    IWebTerminal? webTerminal = _terminalRepository.GetOrDefault(request.TerminalId);

                    if (webTerminal is not null)
                        await webTerminal.ClearAsync();
                }
            }
        }
    }
}
