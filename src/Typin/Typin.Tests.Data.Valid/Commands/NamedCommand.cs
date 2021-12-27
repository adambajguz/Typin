namespace Typin.Tests.Data.Valid.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;
    using Typin.Console;

    [Command("named", Description = "Named command description")]
    public class NamedCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(NamedCommand);

        private readonly IConsole _console;

        public NamedCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine(ExpectedOutputText);

            return default;
        }
    }
}