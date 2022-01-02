﻿namespace TypinExamples.DEVConsole
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    internal class Program
    {
        private const string ENV_VAR = "TARGET_TYPIN_PROJECT";

        private const string CONFIGURATION_FILE = "appsettings.json";


        private static ExampleDescriptor? LoadConfiguration(string exampleName)
        {
            if (!File.Exists(CONFIGURATION_FILE))
            {
                return null;
            }

            string configuration = File.ReadAllText(CONFIGURATION_FILE);
            ExamplesRootConfiguration? options = JsonSerializer.Deserialize<ExamplesRootConfiguration>(configuration);

            ExampleDescriptor? descriptor = options?.Examples?.Descriptors?.Where(x => (x.ProgramClass?.Contains(exampleName) ?? false) ||
                                                                                       (x.Name?.Contains(exampleName) ?? false))
                                                                                .FirstOrDefault();

            return descriptor;
        }

        public static async Task<int?> RunExample(ExampleDescriptor? descriptor)
        {
            if (string.IsNullOrWhiteSpace(descriptor?.ProgramClass))
            {
                return null;
            }

            Type? type = Type.GetType(descriptor.ProgramClass);

            if (type?.GetMethod("Main")?.Invoke(null, null) is Task<int> task)
            {
                int exitCode = await task;

                return exitCode;
            }

            return null;
        }

        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        public static async Task<int> Main(string[] args)
        {
            string target = Environment.GetEnvironmentVariable(ENV_VAR) ?? string.Empty;
            Trace.WriteLine($"TypinExamples.DEVConsole: Starting '{target}' example...");

            ExampleDescriptor? descriptor = LoadConfiguration(target);
            int? exitCode = await RunExample(descriptor);

            if (exitCode is null)
            {
                Trace.WriteLine("TypinExamples.DEVConsole: '{target}' is an invalid example.");
                var prevColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.Write("TypinExamples.DEVConsole: ");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Error.Write("'");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Error.Write(target);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Error.Write("' is an invalid example.");

                Console.ForegroundColor = prevColor;
                Console.Error.WriteLine();
                return 1;
            }

            return (int)exitCode;
        }
    }
}
