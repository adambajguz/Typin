namespace Typin.Commands.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Builders;

    /// <summary>
    /// Inline command configuration proxy.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    internal sealed class InlineConfigureCommand<TCommand> : IConfigureCommand<TCommand>
        where TCommand : class, ICommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, ICommandBuilder<TCommand>, CancellationToken, ValueTask> _configure;

        /// <summary>
        /// Initializes a new instance of <see cref="InlineConfigureCommand{TCommand}"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configure"></param>
        public InlineConfigureCommand(IServiceProvider serviceProvider,
                                      Func<IServiceProvider, ICommandBuilder<TCommand>, CancellationToken, ValueTask> configure)
        {
            _serviceProvider = serviceProvider;
            _configure = configure;
        }

        /// <inheritdoc/>
        public async ValueTask ConfigureAsync(ICommandBuilder<TCommand> builder, CancellationToken cancellationToken)
        {
            await _configure(_serviceProvider, builder, cancellationToken);
        }
    }
}
