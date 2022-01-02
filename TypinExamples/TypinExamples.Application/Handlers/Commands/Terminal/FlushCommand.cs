namespace TypinExamples.Application.Handlers.Commands
{
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public sealed class FlushCommand : ICommand
    {
        public string? TerminalId { get; init; }
    }
}
