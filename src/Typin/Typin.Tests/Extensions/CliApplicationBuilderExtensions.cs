namespace Typin.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Console;
    using Typin.Utilities.CliFx.Utilities;
    using Xunit.Abstractions;

    //TODO: maybe this should be a separate lib like Typin.Testing
    internal static class CliApplicationBuilderExtensions
    {
        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput)
        {
            return await BuildAndRunTestAsync(applicationBuilder, testOutput, Array.Empty<string>(), new Dictionary<string, string>());
        }

        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 IReadOnlyList<string> commandLineArguments)
        {
            return await BuildAndRunTestAsync(applicationBuilder, testOutput, commandLineArguments, new Dictionary<string, string>());
        }

        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 IReadOnlyList<string> commandLineArguments,
                                                                                                                                 IReadOnlyDictionary<string, string> environmentVariables)
        {
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            CliApplication application = applicationBuilder.UseConsole(console)
                                                           .Build();

            int exitCode = await application.RunAsync(commandLineArguments, environmentVariables);

            testOutput.WriteLine("Exit Code: {0}", exitCode);
            testOutput.Print(stdOut, stdErr);

            return (exitCode, stdOut, stdErr);
        }

        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 string commandLine)
        {
            return await BuildAndRunTestAsync(applicationBuilder, testOutput, commandLine, new Dictionary<string, string>());
        }

        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 string commandLine,
                                                                                                                                 IReadOnlyDictionary<string, string> environmentVariables)
        {
            var (console, stdOut, stdErr) = VirtualConsole.CreateBuffered();

            CliApplication application = applicationBuilder.UseConsole(console)
                                                           .Build();

            int exitCode = await application.RunAsync(commandLine, environmentVariables);

            testOutput.WriteLine("Exit Code: {0}", exitCode);
            testOutput.Print(stdOut, stdErr);

            return (exitCode, stdOut, stdErr);
        }
    }
}
