namespace SimpleAppExample.Configurators
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Commands.Builders;

    public sealed class ConfigureCommandFromAttributes<TCommand> : IConfigureCommand<TCommand>
        where TCommand : class, ICommand
    {
        public ValueTask ConfigureAsync(ICommandBuilder<TCommand> builder, CancellationToken cancellationToken)
        {
            builder.FromAttributes();

            return default;
        }
    }
}
