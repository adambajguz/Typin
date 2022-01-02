namespace TypinExamples.Infrastructure.WebWorkers.Common.Payloads
{
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public sealed class RunProgramCommand : ICommand<RunProgramResult>
    {
        public string? ProgramClass { get; init; }
    }
}
