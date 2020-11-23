namespace TypinExamples.Application.Services.TypinWeb
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TypinExamples.Application.Configuration;

    public interface IWebExampleInvokerService
    {
        WebCliConfiguration Configuration { get; }
        ExamplesSettings Options { get; }

        void AttachConsole(IConsole console);
        void AttachLogger(IWebLoggerDestination? loggerDestination);
        Task<int?> Run(ExampleDescriptor? descriptor);
        Task<int?> Run(ExampleDescriptor? descriptor, string commandLine);
        Task<int?> Run(ExampleDescriptor? descriptor, string commandLine, IReadOnlyDictionary<string, string> environmentVariables);
        Task<int?> Run(string? key);
        Task<int?> Run(string? key, string commandLine);
        Task<int?> Run(string? key, string commandLine, IReadOnlyDictionary<string, string> environmentVariables);
    }
}