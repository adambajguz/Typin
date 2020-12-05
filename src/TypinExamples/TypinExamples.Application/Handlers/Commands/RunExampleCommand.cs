namespace TypinExamples.Application.Handlers.Commands
{
    using System.Collections.Generic;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public class RunExampleCommand : ICommand<RunExampleResult>
    {
        public string? TerminalId { get; init; }

        public string? Key { get; init; }
        public string? Args { get; init; }
        public string? ProgramClass { get; init; }
        public string? WebProgramClass { get; init; }
        public Dictionary<string, string> EnvironmentVariables { get; init; } = new();
    }
}
