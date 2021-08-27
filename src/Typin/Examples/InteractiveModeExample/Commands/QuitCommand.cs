namespace InteractiveModeExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Typin;
    using Typin.Attributes;
    using Typin.Modes;

    [Command("quit", Description = "Quits the interactive mode",
             SupportedModes = new[] { typeof(InteractiveMode) })]
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