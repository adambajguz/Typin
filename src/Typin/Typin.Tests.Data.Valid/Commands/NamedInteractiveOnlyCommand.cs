namespace Typin.Tests.Data.Valid.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Console;

    [Command("named-interactive-only", Description = "Named command description")]
    public class NamedInteractiveOnlyCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(NamedInteractiveOnlyCommand);

        private readonly IConsole _console;

        public NamedInteractiveOnlyCommand(IConsole console)
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