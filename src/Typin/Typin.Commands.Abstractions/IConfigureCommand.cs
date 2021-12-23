namespace Typin.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands.Builders;
    using Typin.Models;

    /// <summary>
    /// Represents an object that configures a command <typeparamref name="TCommand"/>.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface IConfigureCommand<TCommand>
        where TCommand : class, ICommand
    {
        /// <summary>
        /// Configure model using a <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ConfigureAsync(ICommandBuilder<TCommand> builder, CancellationToken cancellationToken);
    }
}
