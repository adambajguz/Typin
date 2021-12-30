namespace Typin.Commands.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Builders;

    /// <summary>
    /// Inline global command configuration proxy.
    /// </summary>
    internal sealed class InlineConfigureCommands : IConfigureCommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, ICommandBuilder, CancellationToken, ValueTask> _configure;

        /// <summary>
        /// Initializes a new instance of <see cref="InlineConfigureCommands"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configure"></param>
        public InlineConfigureCommands(IServiceProvider serviceProvider,
                                       Func<IServiceProvider, ICommandBuilder, CancellationToken, ValueTask> configure)
        {
            _serviceProvider = serviceProvider;
            _configure = configure;
        }

        /// <inheritdoc/>
        public async ValueTask ConfigureAsync(ICommandBuilder builder, CancellationToken cancellationToken)
        {
            await _configure(_serviceProvider, builder, cancellationToken);
        }
    }
}
