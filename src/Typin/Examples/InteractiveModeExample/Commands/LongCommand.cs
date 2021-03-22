namespace InteractiveModeExample.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("long", Description = "A long command.")]
    public class LongCommand : ICommand
    {
        public async ValueTask ExecuteAsync(IConsole console)
        {
            await Task.Delay(10_000, console.GetCancellationToken());
        }
    }
}
