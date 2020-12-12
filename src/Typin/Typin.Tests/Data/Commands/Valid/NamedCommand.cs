namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("named", Description = "Named command description")]
    public class NamedCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(NamedCommand);

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WithBackgroundColor(ConsoleColor.Magenta, (output) => output.WriteLine("Magenta"));
            console.Output.WithForegroundColor(ConsoleColor.Green, (output) => output.WriteLine("Green"));
            console.Output.WithColors(ConsoleColor.Red, ConsoleColor.Yellow, (output) => output.WriteLine("Red"));

            console.Output.WriteLine(ExpectedOutputText);

            return default;
        }
    }
}