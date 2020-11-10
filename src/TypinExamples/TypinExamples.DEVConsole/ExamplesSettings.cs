namespace TypinExamples.DEVConsole
{
    using System;

    public sealed class ExamplesSettings
    {
        public ExampleDescriptor[] Examples { get; set; } = Array.Empty<ExampleDescriptor>();
    }

    public sealed class ExampleDescriptor
    {
        public string? Name { get; set; }
        public string? ProgramClass { get; set; }
        public string? Description { get; set; }
    }
}
