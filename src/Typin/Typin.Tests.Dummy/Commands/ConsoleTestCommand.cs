namespace Typin.Tests.Dummy.Commands
{
    using System;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("console-test")]
    public class ConsoleTestCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            string input = console.Input.ReadToEnd();

            console.WithColors(ConsoleColor.Black, ConsoleColor.White, (c) =>
            {
                c.Output.WriteLine(input);
                c.Error.WriteLine(input);
            });

            return default;
        }
    }
}