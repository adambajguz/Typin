namespace InteractiveModeExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("plot xy", Description = "Prints a middleware pipeline structure in application.")]
    public class PlotXYCommand : ICommand
    {
        private readonly ICliContextAccessor _cliContextAccessor;
        private readonly IConsole _console;

        public PlotXYCommand(ICliContextAccessor cliContextAccessor, IConsole console)
        {
            _cliContextAccessor = cliContextAccessor;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine($"plot xy | {_cliContextAccessor.CliContext!.Depth}");

            return default;
        }
    }
}
