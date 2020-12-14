namespace TypinExamples.Application.Configurations
{
    using System;

    public sealed class ExamplesConfiguration
    {
        public ExampleDescriptor[] Descriptors { get; init; } = Array.Empty<ExampleDescriptor>();
    }
}
