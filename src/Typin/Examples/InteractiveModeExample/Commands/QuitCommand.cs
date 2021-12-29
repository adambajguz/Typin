namespace InteractiveModeExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Typin.Commands;
    using Typin.Commands.Attributes;

    [Command("quit", Description = "Quits the interactive mode")]
    public class QuitCommand : ICommand
    {
        private readonly IHostApplicationLifetime _lifetime;

        public QuitCommand(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _lifetime.StopApplication();

            return default;
        }
    }
}