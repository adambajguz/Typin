namespace TypinExamples.DEVConsole
{
    using System;

    public sealed class Configuration
    {
        public ExamplesConfiguration? Examples { get; init; }
    }

    public sealed class ExamplesConfiguration
    {
        public ExampleDescriptor[] Descriptors { get; init; } = Array.Empty<ExampleDescriptor>();
    }

    public sealed class ExampleDescriptor
    {
        public string? Name { get; init; }
        public string? ProgramClass { get; init; }
    }
}
