namespace TypinExamples.Infrastructure.TypinWeb.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Console;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;

    public interface IWebExampleInvokerService
    {
        WebCliConfiguration Configuration { get; }

        void AttachConsole(IConsole console);
        void AttachLogger(IWebLoggerDestination? loggerDestination);
        Task<int> Run(string webProgramClass, string commandLine, IReadOnlyDictionary<string, string> environmentVariables);
    }
}