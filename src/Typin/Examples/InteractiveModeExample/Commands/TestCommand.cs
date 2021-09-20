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
        private readonly ICommandExecutor _commandExecutor;

        [Option("xe", 'a')]
        public string Author { get; init; } = string.Empty;

        [Option('x')]
        public string AuthorX { get; init; } = string.Empty;

        [Option("char", 'c')]
        public char Ch { get; init; }

        [Option("date", 'd')]
        public DateTime Date { get; init; } = DateTime.Now;

        public TestCommand(IConsole console, ICliContextAccessor cliContextAccessor, ICommandExecutor commandExecutor)
        {
            _console = console;
            _cliContextAccessor = cliContextAccessor;
            _commandExecutor = commandExecutor;
        }

        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine($"ContextId: {_cliContextAccessor.CliContext!.Id} | '{Author}' '{AuthorX}' '{Ch}'");

            if (_cliContextAccessor.CliContext.Depth < 10)
            {
                await _commandExecutor.ExecuteAsync(_cliContextAccessor.CliContext.Input!.Arguments, CommandExecutionOptions.Default, cancellationToken);
            }
        }
    }
}
