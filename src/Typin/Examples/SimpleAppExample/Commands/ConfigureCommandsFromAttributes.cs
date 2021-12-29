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
            builder.FromAttribute(); //TODO: called twice

            return default;
        }
    }

    public sealed class ConfigureCommandsFromAttributes<TCommand> : IConfigureCommand<TCommand>
        where TCommand : class, ICommand
    {
        public ValueTask ConfigureAsync(ICommandBuilder<TCommand> builder, CancellationToken cancellationToken)
        {
            builder.FromAttribute();

            return default;
        }
    }
}
