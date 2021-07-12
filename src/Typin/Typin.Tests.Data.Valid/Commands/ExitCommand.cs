namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Command("exit", Description = "Exits.")]
    public class ExitCommand : ICommand
    {
        private readonly ICliApplicationLifetime _lifetime;

        public ExitCommand(ICliApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _lifetime.RequestStop();

            return default;
        }
    }
}
