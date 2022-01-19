namespace InteractiveModeExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Schemas.Attributes;

    [Alias("long")]
    public class LongCommand : ICommand
    {
        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(10_000, cancellationToken);
        }
    }
}
