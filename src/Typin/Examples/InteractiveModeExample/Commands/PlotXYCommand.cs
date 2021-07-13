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
        private readonly IConsole _console;

        public PlotXYCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine("plot xy");

            return default;
        }
    }
}
