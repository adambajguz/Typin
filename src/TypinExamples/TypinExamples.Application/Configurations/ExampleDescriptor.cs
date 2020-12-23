namespace TypinExamples.Application.Configurations
{
    public sealed class ExampleDescriptor
    {
        public string? Key { get; init; }
        public string? Name { get; init; }
        public bool IsNew { get; init; }
        public string[]? Modes { get; init; }

        public string? DownloadPath { get; init; }
        public string? DownloadFile { get; init; }

        public string? ProgramClass { get; init; }
        public string? WebProgramClass { get; init; }

        public string? Description { get; init; }
        public string[]? QuickStart { get; init; }
        public string QuickStartText => QuickStart is null ? string.Empty : string.Join('\n', QuickStart);

        public override string? ToString()
        {
            return $"('{Key}', '{Name}', '{ProgramClass}', '{WebProgramClass}')";
        }
    }
}
