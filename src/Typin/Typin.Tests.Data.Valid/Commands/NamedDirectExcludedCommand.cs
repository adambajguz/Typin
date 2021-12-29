namespace Typin.Tests.Data.Valid.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Console;

    [Command("named-direct-excluded-only", Description = "Named command description")]
    public class NamedDirectExcludedCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(NamedDirectExcludedCommand);

        private readonly IConsole _console;

        public NamedDirectExcludedCommand(IConsole console)
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