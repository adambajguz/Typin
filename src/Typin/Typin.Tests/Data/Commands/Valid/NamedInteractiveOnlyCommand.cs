namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("named-interactive-only", Description = "Named command description",
             SupportedModes = new[] { typeof(InteractiveMode) })]
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