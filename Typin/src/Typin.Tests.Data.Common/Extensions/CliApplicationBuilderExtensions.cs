namespace Typin.Tests.Data.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Console;
    using Typin.Console.IO;
    using Xunit.Abstractions;

    //TODO: maybe this should be a separate lib like Typin.Testing
    public static class CliApplicationBuilderExtensions
    {

        #region Array based
        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 bool isInputRedirected = true,
                                                                                                                                 string? input = null)
        {
            return await applicationBuilder.BuildAndRunTestAsync(testOutput,
                                              Array.Empty<string>(),
                                              new Dictionary<string, string>(),
                                              isInputRedirected,
                                              input);
        }

        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 IReadOnlyList<string> commandLineArguments,
                                                                                                                                 bool isInputRedirected = true,
                                                                                                                                 string? input = null)
        {
            return await applicationBuilder.BuildAndRunTestAsync(testOutput,
                                              commandLineArguments,
                                              new Dictionary<string, string>(),
                                              isInputRedirected,
                                              input);
        }

        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 IReadOnlyList<string> commandLineArguments,
                                                                                                                                 IReadOnlyDictionary<string, string> environmentVariables,
                                                                                                                                 bool isInputRedirected = true,
                                                                                                                                 string? input = null)
        {
            var (console, stdIn, stdOut, stdErr) = VirtualConsole.CreateBufferedWithInput(isInputRedirected: isInputRedirected);

            CliApplication application = applicationBuilder.UseConsole(console)
                                                           .Build();

            if (input is not null)
            {
                stdIn.WriteString(input.TrimEnd('\r') + "\rexit\r");
            }

            int exitCode = await application.RunAsync(commandLineArguments, environmentVariables);

            testOutput.WriteLine("Exit Code: {0}", exitCode);
            testOutput.Print(stdOut, stdErr);

            return (exitCode, stdOut, stdErr);
        }
        #endregion

        #region String based
        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 string commandLine,
                                                                                                                                 bool containsExecutable = false,
                                                                                                                                 bool isInputRedirected = true,
                                                                                                                                 string? input = null)
        {
            return await applicationBuilder.BuildAndRunTestAsync(testOutput,
                                              commandLine,
                                              new Dictionary<string, string>(),
                                              containsExecutable,
                                              isInputRedirected,
                                              input);
        }

        public static async ValueTask<(int exitCode, MemoryStreamWriter stdOut, MemoryStreamWriter stdErr)> BuildAndRunTestAsync(this CliApplicationBuilder applicationBuilder,
                                                                                                                                 ITestOutputHelper testOutput,
                                                                                                                                 string commandLine,
                                                                                                                                 IReadOnlyDictionary<string, string> environmentVariables,
                                                                                                                                 bool containsExecutable = false,
                                                                                                                                 bool isInputRedirected = true,
                                                                                                                                 string? input = null)
        {
            var (console, stdIn, stdOut, stdErr) = VirtualConsole.CreateBufferedWithInput(isInputRedirected: isInputRedirected);

            CliApplication application = applicationBuilder.UseConsole(console)
                                                           .Build();

            if (input is not null)
            {
                stdIn.WriteString(input.TrimEnd('\r') + "\rexit\r");
            }

            int exitCode = await application.RunAsync(commandLine, environmentVariables, containsExecutable);

            testOutput.WriteLine("Exit Code: {0}", exitCode);
            testOutput.Print(stdOut, stdErr);

            return (exitCode, stdOut, stdErr);
        }
        #endregion
    }
}