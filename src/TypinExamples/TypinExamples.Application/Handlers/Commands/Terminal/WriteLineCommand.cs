namespace TypinExamples.Application.Handlers.Commands.Terminal
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TypinExamples.Application.Services;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Domain.Interfaces.Handlers.Core;

    public class WriteLineCommand : ICoreRequest<Unit>
    {
        public long? WorkerId { get; set; }

        public string? TerminalId { get; init; }
        public string? Value { get; init; }

        public class WriteLineHandler : ICoreRequestHandler<WriteLineCommand>
        {
            private readonly ITerminalRepository _terminalRepository;

            public WriteLineHandler(ITerminalRepository terminalRepository)
            {
                _terminalRepository = terminalRepository;
            }

            public async Task<Unit> Handle(WriteLineCommand request, CancellationToken cancellationToken)
            {
                if (request.TerminalId is not null && request.Value is not null)
                {
                    IWebTerminal? webTerminal = _terminalRepository.GetOrDefault(request.TerminalId);

                    if (webTerminal is not null)
                        await webTerminal.WriteLineAsync(request.Value);
                }

                return Unit.Value;
            }
        }
    }
}
