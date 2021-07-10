namespace Typin.Tests.Dummy.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("console-test")]
    public class ConsoleTestCommand : ICommand
    {
        private readonly IConsole _console;

        public ConsoleTestCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            string input = _console.Input.ReadToEnd();

            _console.WithColors(ConsoleColor.Black, ConsoleColor.White, (c) =>
            {
                c.Output.WriteLine(input);
                c.Error.WriteLine(input);
            });

            return default;
        }
    }
}