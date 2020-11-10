namespace TypinExamples.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Typin.Console;
    using TypinExamples.Configuration;

    public class ExampleRunnerService
    {
        public ExamplesSettings Options { get; }

        public ExampleRunnerService(IOptions<ExamplesSettings> options)
        {
            Options = options.Value;
        }

        public int? Run(string exampleName, IConsole console, IReadOnlyList<string> commandLineArguments, IReadOnlyDictionary<string, string> environmentVariables)
        {
            ExampleDescriptor? descriptor = Options.Examples?.Where(x => (x.ProgramClass?.Contains(exampleName) ?? false) ||
                                                                         (x.Name?.Contains(exampleName) ?? false))
                                                             .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(descriptor?.ProgramClass))
                return null;

            Type? type = Type.GetType(descriptor.ProgramClass);

            Task<int>? task = type?.GetMethod("WebMain")?.Invoke(null, new object[] { console, commandLineArguments, environmentVariables }) as Task<int>;
            int? exitCode = task?.GetAwaiter().GetResult(); //TODO fix

            return exitCode;
        }
    }
}
