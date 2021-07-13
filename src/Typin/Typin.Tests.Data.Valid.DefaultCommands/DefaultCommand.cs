namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Default command description")]
    public class DefaultCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(DefaultCommand);

        private readonly IConsole _console;

        public DefaultCommand(IConsole console)
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