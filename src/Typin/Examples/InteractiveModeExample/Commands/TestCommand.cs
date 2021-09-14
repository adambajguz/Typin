namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("test", Description = "Test command.")]
    public class TestCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly ICliContextAccessor _cliContextAccessor;
        private readonly ICliCommandExecutor _commandExecutor;

        [Option("xe", 'a')]
        public string Author { get; init; } = string.Empty;

        [Option('x')]
        public string AuthorX { get; init; } = string.Empty;

        [Option("char", 'c')]
        public char Ch { get; init; }

        [Option("date", 'd')]
        public DateTime Date { get; init; } = DateTime.Now;

        public TestCommand(IConsole console, ICliContextAccessor cliContextAccessor, ICliCommandExecutor commandExecutor)
        {
            _console = console;
            _cliContextAccessor = cliContextAccessor;
            _commandExecutor = commandExecutor;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine($"'{Author}' '{AuthorX}' '{Ch}'");

            //await _commandExecutor.ExecuteCommandAsync(_cliContextAccessor.CliContext!.Input!.Arguments, false, cancellationToken);

            return default;
        }
    }
}
