namespace TypinExamples.Timer.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("long")]
    public class LongRunningCommand : ICommand
    {
        [CommandParameter(0)]
        public int? Time { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(nameof(LongRunningCommand));
            await Task.Delay(Time ?? 0);
        }
    }
}