namespace TypinExamples.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin.Console;
    using TypinExamples.Configuration;
    using TypinExamples.TypinWeb.Configuration;

    public class ExampleRunnerService
    {
        private readonly ILogger<ExampleRunnerService> _logger;

        public ExamplesSettings Options { get; }
        public WebCliConfiguration Configuration { get; }

        public ExampleRunnerService(IOptions<ExamplesSettings> options, ILogger<ExampleRunnerService> logger)
        {
            Options = options.Value;
            _logger = logger;
            Configuration = new WebCliConfiguration();
        }

        public void AttachConsole(IConsole console)
        {
            Configuration.Console = console;
        }

        public void AttachLoggere(ILoggerProvider loggerProvider)
        {
            Configuration.LoggerProvider = loggerProvider;
        }

        public async Task<int?> Run(string exampleName)
        {
            return await Run(exampleName, string.Empty, new Dictionary<string, string>());
        }

        public async Task<int?> Run(string exampleName, string commandLine)
        {
            return await Run(exampleName, commandLine, new Dictionary<string, string>());
        }

        public async Task<int?> Run(string exampleName, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            ExampleDescriptor? descriptor = Options.Examples?.Where(x => (x.ProgramClass?.Contains(exampleName) ?? false) ||
                                                                         (x.Name?.Contains(exampleName) ?? false))
                                                             .FirstOrDefault();

            return await Run(descriptor, commandLine, environmentVariables);
        }

        public async Task<int?> Run(ExampleDescriptor? descriptor)
        {
            return await Run(descriptor, string.Empty, new Dictionary<string, string>());
        }

        public async Task<int?> Run(ExampleDescriptor? descriptor, string commandLine)
        {
            return await Run(descriptor, commandLine, new Dictionary<string, string>());
        }

        public async Task<int?> Run(ExampleDescriptor? descriptor, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            if (Configuration.Console is null)
            {
                _logger.LogError("Console was not attached.");
                return null;
            }

            if (string.IsNullOrWhiteSpace(descriptor?.ProgramClass))
            {
                _logger.LogError($"Invalid example descriptor '{descriptor}'.");
                return null;
            }

            Type? type = Type.GetType(descriptor.ProgramClass);
            Task<int>? task = type?.GetMethod("WebMain")?.Invoke(null, new object[] { Configuration, commandLine, environmentVariables }) as Task<int>;

            int? exitCode = task == null ? null : await task;
            if (exitCode is null)
            {
                _logger.LogError($"Failed to run {descriptor}.");
            }

            return exitCode;
        }
    }
}
