namespace TypinExamples.Configuration
{
    using System;

    public sealed class ExamplesSettings
    {
        public ExampleDescriptor[] Examples { get; init; } = Array.Empty<ExampleDescriptor>();
    }

    public sealed class ExampleDescriptor
    {
        public string? Page { get; init; }
        public string? Name { get; init; }
        public bool IsNew { get; init; }
        public string? ProgramClass { get; init; }
        public string? Description { get; init; }
        public string? QuickStart { get; init; }

        public override string? ToString()
        {
            return $"<'{Page}', '{Name}', '{ProgramClass}'>";
        }
    }
}
