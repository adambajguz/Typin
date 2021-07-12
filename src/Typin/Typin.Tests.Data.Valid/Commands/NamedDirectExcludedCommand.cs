namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("named-direct-excluded-only", Description = "Named command description",
             ExcludedModes = new[] { typeof(DirectMode) })]
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