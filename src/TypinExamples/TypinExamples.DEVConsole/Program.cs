namespace TypinExamples.DEVConsole
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    internal class Program
    {
        private const string ENV_VAR = "TARGET_TYPIN_PROJECT";

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "May be needed for examples.")]
        public static async Task<int> Main(string[] args)
        {
            string target = Environment.GetEnvironmentVariable(ENV_VAR) ?? string.Empty;
            Trace.WriteLine($"TypinExamples.DEVConsole: Starting '{target}' example...");

            switch (target)
            {
                case "TypinExamples.HelloWorld.Program":
                    return await HelloWorld.Program.Main();

                case "TypinExamples.CalculatOR.Program":
                    return await CalculatOR.Program.Main();

                case "TypinExamples.Timer.Program":
                    return await Timer.Program.Main();

                case "TypinExamples.InteractiveQuery.Program":
                    return await InteractiveQuery.Program.Main();

                case "TypinExamples.MarioBuilder.Program":
                    return await MarioBuilder.Program.Main();

                default:
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
        }
    }
}
