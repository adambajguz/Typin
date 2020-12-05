namespace TypinExamples.Infrastructure.TypinWeb.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;
    using TypinExamples.Infrastructure.TypinWeb.Console;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public sealed class WebExampleInvoker : IDisposable
    {
        public WebConsole Console { get; }
        public IWebLoggerDestination? LoggerDestination { get; private set; }

        public WebExampleInvoker(IWorker worker, string terminalId, CancellationToken cancellationToken = default)
        {
            Console = new WebConsole(worker, terminalId, cancellationToken);
        }

        public void AttachLogger(IWebLoggerDestination? loggerDestination)
        {
            LoggerDestination = loggerDestination;
        }

        public async Task<int> Run(string webProgramClass, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            if (string.IsNullOrWhiteSpace(webProgramClass))
            {
                throw new ArgumentException($"Invalid web program main class '{webProgramClass}'.");
            }

            Type type = Type.GetType(webProgramClass) ?? throw new ApplicationException($"Failed to run {webProgramClass} (invalid web program class type).");
            Task<int>? task = type.GetMethod("WebMain")?.Invoke(null, BuildWebProgramMainArgs(commandLine, environmentVariables)) as Task<int>;

            int exitCode = task is null ? throw new ApplicationException($"Failed to run {webProgramClass} ('WebMain' method not found).") : await task;

            return exitCode;
        }

        private object[] BuildWebProgramMainArgs(string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return new object[] { new WebCliConfiguration(Console, LoggerDestination), commandLine, environmentVariables };
        }

        public void Dispose()
        {
            Console.Dispose();
        }
    }
}
