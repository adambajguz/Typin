namespace InteractiveModeExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;
    using Typin.Console;

    [Command("plot xyz")]
    public class PlotXYZCommand : ICommand
    {
        private readonly IConsole _console;

        public PlotXYZCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine("plot xyz");

            return default;
        }
    }
}
