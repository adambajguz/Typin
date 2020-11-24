namespace TypinExamples.Infrastructure.TypinWeb.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Console;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;

    public class WebExampleInvokerService : IWebExampleInvokerService
    {
        public WebCliConfiguration Configuration { get; } = new();

        public WebExampleInvokerService()
        {

        }

        public void AttachConsole(IConsole console)
        {
            Configuration.Console = console;
        }

        public void AttachLogger(IWebLoggerDestination? loggerDestination)
        {
            Configuration.LoggerDestination = loggerDestination;
        }

        public async Task<int> Run(string webProgramClass, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            if (Configuration.Console is null)
            {
                throw new ApplicationException("Console was not attached.");
            }

            if (string.IsNullOrWhiteSpace(webProgramClass))
            {
                throw new ApplicationException($"Invalid web program main class '{webProgramClass}'.");
            }

            Type? type = Type.GetType(webProgramClass);
            Task<int>? task = type?.GetMethod("WebMain")?.Invoke(null, BuildWebProgramMainArgs(commandLine, environmentVariables)) as Task<int>;

            int? exitCode = task == null ? null : await task;
            if (exitCode is null)
                throw new ApplicationException($"Failed to run {webProgramClass}.");

            return (int)exitCode;
        }

        private object[] BuildWebProgramMainArgs(string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return new object[] { Configuration, commandLine, environmentVariables };
        }
    }
}
