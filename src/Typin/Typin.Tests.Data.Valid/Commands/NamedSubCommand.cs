namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("named sub", Description = "Named sub command description")]
    public class NamedSubCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(NamedSubCommand);

        private readonly IConsole _console;

        public NamedSubCommand(IConsole console)
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