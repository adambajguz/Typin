namespace Typin.Tests.Dummy.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;

    [Command]
    public class HelloWorldCommand : ICommand
    {
        private readonly IConsole _console;

        [Option("target")]
        public string Target { get; init; } = "World";

        public HelloWorldCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine($"Hello {Target}!");

            return default;
        }
    }
}