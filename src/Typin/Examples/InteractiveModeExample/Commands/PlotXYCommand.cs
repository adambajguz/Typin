namespace Typin.InteractiveModeDemo.Commands
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("plot xy", Description = "Prints a middleware pipeline structure in application.")]
    public class PlotXYCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("plot xy");

            return default;
        }
    }
}
