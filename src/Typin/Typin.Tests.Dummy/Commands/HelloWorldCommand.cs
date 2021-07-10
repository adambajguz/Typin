namespace Typin.Tests.Dummy.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class HelloWorldCommand : ICommand
    {
        private readonly IConsole _console;

        [Option("target", FallbackVariableName = "ENV_TARGET")]
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