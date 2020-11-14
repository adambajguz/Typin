namespace TypinExamples.CalculatOR
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .Build()
                                                    .RunAsync();
        }

        public static async Task<int> WebMain(IConsole console, IReadOnlyList<string> commandLineArguments, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .UseConsole(console)
                                                    .Build()
                                                    .RunAsync(commandLineArguments, environmentVariables);
        }
    }
}
