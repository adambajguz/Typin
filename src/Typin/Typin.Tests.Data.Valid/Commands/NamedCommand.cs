namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
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