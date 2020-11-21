namespace TypinExamples.Application.Configuration
{
    using System;

    public sealed class ExamplesSettings
    {
        public ExampleDescriptor[] Examples { get; init; } = Array.Empty<ExampleDescriptor>();
    }
}
