namespace Typin.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Console;
    using Xunit.Abstractions;

    internal static class CliApplicationExtensions
    {
        public static async ValueTask<int> RunTestAsync(this CliApplication application,
                                                        VirtualConsole virtualConsole,
                                                        ITestOutputHelper testOutput)
        {
            return await RunTestAsync(application, virtualConsole, testOutput, Array.Empty<string>(), new Dictionary<string, string>());
        }

        public static async ValueTask<int> RunTestAsync(this CliApplication application,
                                                        VirtualConsole virtualConsole,
                                                        ITestOutputHelper testOutput,
                                                        IReadOnlyList<string> commandLineArguments)
        {
            return await RunTestAsync(application, virtualConsole, testOutput, commandLineArguments, new Dictionary<string, string>());
        }

        public static async ValueTask<int> RunTestAsync(this CliApplication application,
                                                        VirtualConsole virtualConsole,
                                                        ITestOutputHelper testOutput,
                                                        IReadOnlyList<string> commandLineArguments,
                                                        IReadOnlyDictionary<string, string> environmentVariables)
        {
            int exitCode = await application.RunAsync(commandLineArguments, environmentVariables);

            testOutput.WriteLine("Exit Code: {0}", exitCode);
            testOutput.Print(virtualConsole);

            return exitCode;
        }
    }
}
