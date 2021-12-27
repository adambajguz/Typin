namespace InteractiveModeExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;

    [Command("long", Description = "A long command.")]
    public class LongCommand : ICommand
    {
        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(10_000, cancellationToken);
        }
    }
}
