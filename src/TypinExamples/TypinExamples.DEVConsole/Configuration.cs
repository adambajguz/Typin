namespace TypinExamples.DEVConsole
{
    using System;

    public sealed class Configuration
    {
        public ExamplesSettings? ExamplesSettings { get; init; }
    }

    public sealed class ExamplesSettings
    {
        public ExampleDescriptor[] Examples { get; init; } = Array.Empty<ExampleDescriptor>();
    }

    public sealed class ExampleDescriptor
    {
        public string? Name { get; init; }
        public string? ProgramClass { get; init; }
    }
}
