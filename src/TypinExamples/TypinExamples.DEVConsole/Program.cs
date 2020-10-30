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
            switch (Environment.GetEnvironmentVariable(ENV_VAR) ?? string.Empty)
            {
                case "TypinExamples.HelloWorld.Program":
                    Debug.WriteLine("Starting 'TypinExamples.HelloWorld.Program' example...");
                    return await HelloWorld.Program.Main();

                case "TypinExamples.CalculatOR.Program":
                    Debug.WriteLine("Starting 'TypinExamples.CalculatOR.Program' example...");
                    return await CalculatOR.Program.Main();

                case "TypinExamples.Timer.Program":
                    Debug.WriteLine("Starting 'TypinExamples.Timer.Program' example...");
                    return await Timer.Program.Main();

                case "TypinExamples.InteractiveQuery.Program":
                    Debug.WriteLine("Starting 'TypinExamples.InteractiveQuery.Program' example...");
                    return await InteractiveQuery.Program.Main();

                case "TypinExamples.MarioBuilder.Program":
                    Debug.WriteLine("Starting 'TypinExamples.MarioBuilder.Program' example...");
                    return await MarioBuilder.Program.Main();

                default:
                    Debug.WriteLine("TypinExamples.DEVConsole: Invalid example.");
                    Console.Error.WriteLine("TypinExamples.DEVConsole: Invalid example.");
                    return 1;
            }
        }
    }
}
