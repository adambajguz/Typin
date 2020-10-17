namespace Typin.InteractiveModeDemo.Commands
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("plot xyz")]
    public class PlotXYZCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("plot xyz");

            return default;
        }
    }
}
