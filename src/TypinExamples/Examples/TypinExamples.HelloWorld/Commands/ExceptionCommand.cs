namespace TypinExamples.HelloWorld.Commands
{
    using System;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("exception", Description = "Demonstration of unknown exception printing.")]
    public class ExceptionCommand : ICommand
    {
        private static readonly string IntentionalExceptionMessage = $"DO NOT take this exception seriously as it is just an example of stack trace printing.";

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteAsync(IntentionalExceptionMessage);
            await console.Output.WriteLineAsync(" Switch to Log Viewer and see all logs.");

            throw new OverflowException(IntentionalExceptionMessage);
        }
    }
}