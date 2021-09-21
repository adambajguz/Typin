namespace InteractiveModeExample.Commands
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes.Programmatic;

    [Command("test", Description = "Test command.")]
    public class TestCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly ICliContextAccessor _cliContextAccessor;
        private readonly ICommandExecutor _commandExecutor;
        private readonly ICliModeSwitcher _cliModeSwitcher;

        [Option("xe", 'a')]
        public string Author { get; init; } = string.Empty;

        [Option('x')]
        public string AuthorX { get; init; } = string.Empty;

        [Option("char", 'c')]
        public char Ch { get; init; }

        [Option("date", 'd')]
        public DateTime Date { get; init; } = DateTime.Now;

        public TestCommand(IConsole console, ICliContextAccessor cliContextAccessor, ICommandExecutor commandExecutor, ICliModeSwitcher cliModeSwitcher)
        {
            _console = console;
            _cliContextAccessor = cliContextAccessor;
            _commandExecutor = commandExecutor;
            _cliModeSwitcher = cliModeSwitcher;
        }

        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            _console.Output.WriteLine($"ContextId: {_cliContextAccessor.CliContext!.Depth} | '{Author}' '{AuthorX}' '{Ch}'");

            await _commandExecutor.ExecuteAsync("plot xy", CommandExecutionOptions.UseCurrentScope, cancellationToken);

            _console.Output.WithForegroundColor(ConsoleColor.Cyan, output => output.WriteLine("- - - - - - -"));

            await _cliModeSwitcher.WithModeAsync<ProgrammaticMode>(async (mode, ct) =>
            {
                mode.Queue(new[] { "plot", "xy" }, 19);
                mode.ExecutionOptions = CommandExecutionOptions.UseCurrentScope;

                await mode.ExecuteAsync(ct);
            }, cancellationToken);

            stopwatch.Stop();

            _console.Output.WriteLine();
            _console.Output.WriteLine($"Elapsed: {stopwatch.Elapsed}");
        }
    }
}
