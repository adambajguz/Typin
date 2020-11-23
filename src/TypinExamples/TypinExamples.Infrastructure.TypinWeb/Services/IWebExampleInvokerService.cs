namespace TypinExamples.Infrastructure.TypinWeb.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Console;
    using TypinExamples.Application.Configuration;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;

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