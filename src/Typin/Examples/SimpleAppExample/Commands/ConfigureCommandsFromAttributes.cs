namespace SimpleAppExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Commands.Builders;

    public sealed class ConfigureCommandsFromAttributes : IConfigureCommand
    {
        public ValueTask ConfigureAsync(ICommandBuilder builder, CancellationToken cancellationToken)
        {
            builder.FromAttribute();

            return default;
        }
    }
}
