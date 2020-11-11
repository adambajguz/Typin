namespace TypinExamples.InteractiveQuery
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;
    using Typin.Directives;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddDirective<DebugDirective>()
                                                    .AddDirective<PreviewDirective>()
                                                    .Build()
                                                    .RunAsync();
        }

        public static async Task<int> WebMain(IConsole console, IReadOnlyList<string> commandLineArguments, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddDirective<DebugDirective>()
                                                    .AddDirective<PreviewDirective>()
                                                    .UseConsole(console)
                                                    .Build()
                                                    .RunAsync(commandLineArguments, environmentVariables);
        }
    }
}
