namespace TypinExamples.Timer.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("long", Description = "Command to simulate long runnig task by passing a ms delay.")]
    public class LongRunningCommand : ICommand
    {
        [CommandParameter(0, Description = "Delay in ms.")]
        public int? Time { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(nameof(LongRunningCommand));
            await Task.Delay(Time ?? 0);
        }
    }
}