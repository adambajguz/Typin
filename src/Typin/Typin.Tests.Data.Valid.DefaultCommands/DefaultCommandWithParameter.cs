namespace Typin.Tests.Data.Valid.DefaultCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;
    using Typin.Console;

    [Command(Description = "Default command with parameter description")]
    public class DefaultCommandWithParameter : ICommand
    {
        public const string ExpectedOutputText = nameof(DefaultCommandWithParameter);

        private readonly IConsole _console;

        [Parameter(0)]
        public string? ParamA { get; init; }

        public DefaultCommandWithParameter(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine(ExpectedOutputText);
            _console.Output.WriteLine(ParamA);

            return default;
        }
    }
}