namespace InteractiveModeExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Typin.Commands;
    using Typin.Schemas.Attributes;

    [Alias("exit")]
    [Alias("quit")]
    [Alias("kill")]
    public class ExitCommand : ICommand
    {
        private readonly IHostApplicationLifetime _lifetime;

        public ExitCommand(IHostApplicationLifetime lifetime)
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
