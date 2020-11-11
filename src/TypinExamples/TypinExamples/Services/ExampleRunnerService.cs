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

    public class ExampleRunnerService
    {
        private readonly ILogger<ExampleRunnerService> _logger;

        public IConsole? Console { get; private set; }
        public ExamplesSettings Options { get; }

        public ExampleRunnerService(IOptions<ExamplesSettings> options, ILogger<ExampleRunnerService> logger)
        {
            Options = options.Value;
            _logger = logger;
        }

        public void AttachConsole(IConsole console)
        {
            Console = console;
        }

        public int? Run(string exampleName)
        {
            return Run(exampleName, new List<string>(), new Dictionary<string, string>());
        }

        public int? Run(string exampleName, IReadOnlyList<string> commandLineArguments)
        {
            return Run(exampleName, commandLineArguments, new Dictionary<string, string>());
        }

        public int? Run(string exampleName, IReadOnlyList<string> commandLineArguments, IReadOnlyDictionary<string, string> environmentVariables)
        {
            ExampleDescriptor? descriptor = Options.Examples?.Where(x => (x.ProgramClass?.Contains(exampleName) ?? false) ||
                                                                         (x.Name?.Contains(exampleName) ?? false))
                                                             .FirstOrDefault();

            return Run(descriptor, commandLineArguments, environmentVariables);
        }

        public int? Run(ExampleDescriptor? descriptor)
        {
            return Run(descriptor, new List<string>(), new Dictionary<string, string>());
        }

        public int? Run(ExampleDescriptor? descriptor, IReadOnlyList<string> commandLineArguments)
        {
            return Run(descriptor, commandLineArguments, new Dictionary<string, string>());
        }

        public int? Run(ExampleDescriptor? descriptor, IReadOnlyList<string> commandLineArguments, IReadOnlyDictionary<string, string> environmentVariables)
        {
            if (Console is null)
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

            Task<int>? task = type?.GetMethod("WebMain")?.Invoke(null, new object[] { Console, commandLineArguments, environmentVariables }) as Task<int>;
            int? exitCode = task?.GetAwaiter().GetResult(); //TODO fix

            if(exitCode is null)
            {
                _logger.LogError($"Failed to run {descriptor}.");
            }

            return exitCode;
        }
    }
}
