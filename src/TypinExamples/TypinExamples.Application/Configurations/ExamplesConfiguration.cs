namespace TypinExamples.Application.Configurations
{
    using System;

    public sealed class ExamplesConfiguration
    {
        public string? SrcFilesRoot { get; init; }
        public ExampleDescriptor[] Descriptors { get; init; } = Array.Empty<ExampleDescriptor>();
    }
}
