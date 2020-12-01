namespace TypinExamples.Application.Handlers.Commands
{
    public class RunExampleCommand
    {
        public string? TerminalId { get; init; }

        public string? Key { get; init; }
        public string? Args { get; init; }
        public string? ProgramClass { get; init; }
        public string? WebProgramClass { get; init; }
    }
}
