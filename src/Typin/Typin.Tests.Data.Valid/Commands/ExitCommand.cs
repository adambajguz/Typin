namespace Typin.Tests.Data.Valid.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Typin.Commands;
    using Typin.Commands.Attributes;

    [Command("exit", Description = "Exits.")]
    public class ExitCommand : ICommand
    {
        private readonly IHostLifetime _lifetime;

        public ExitCommand(IHostLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _lifetime.StopAsync(cancellationToken);

            return default;
        }
    }
}
