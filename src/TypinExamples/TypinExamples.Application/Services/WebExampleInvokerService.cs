namespace TypinExamples.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin.Console;
    using TypinExamples.Application.Configuration;
    using TypinExamples.TypinWeb.Configuration;
    using TypinExamples.TypinWeb.Logging;

    public class WebExampleInvokerService
    {
        private readonly ILogger _logger;

        public ExamplesSettings Options { get; }
        public WebCliConfiguration Configuration { get; }

        public WebExampleInvokerService(IOptions<ExamplesSettings> options, ILogger<WebExampleInvokerService> logger)
        {
            Options = options.Value;
            _logger = logger;

            Configuration = new WebCliConfiguration();
        }

        public void AttachConsole(IConsole console)
        {
            Configuration.Console = console;
        }

        public void AttachLogger(IWebLoggerDestination? loggerDestination)
        {
            Configuration.LoggerDestination = loggerDestination;
        }

        public async Task<int?> Run(string? key)
        {
            return await Run(key, string.Empty, new Dictionary<string, string>());
        }

        public async Task<int?> Run(string? key, string commandLine)
        {
            return await Run(key, commandLine, new Dictionary<string, string>());
        }

        public async Task<int?> Run(string? key, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {

            ExampleDescriptor? descriptor = Options.Examples?.Where(x => x.Key == key ||
                                                                         (x.WebProgramClass?.Contains(key ?? string.Empty) ?? false) ||
                                                                         (x.Name?.Contains(key ?? string.Empty) ?? false))
                                                             .FirstOrDefault() ?? ExampleDescriptor.CreateDynamic();

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

            if (string.IsNullOrWhiteSpace(descriptor?.WebProgramClass))
            {
                _logger.LogError($"Invalid example descriptor '{descriptor}'.");
                return null;
            }

            Type? type = Type.GetType(descriptor.WebProgramClass);
            Task<int>? task = type?.GetMethod("WebMain")?.Invoke(null, BuildWebProgramMainArgs(commandLine, environmentVariables)) as Task<int>;

            int? exitCode = task == null ? null : await task;
            if (exitCode is null)
            {
                _logger.LogError($"Failed to run {descriptor}.");
            }

            return exitCode;
        }

        private object[] BuildWebProgramMainArgs(string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return new object[] { Configuration, commandLine, environmentVariables };
        }
    }
}
