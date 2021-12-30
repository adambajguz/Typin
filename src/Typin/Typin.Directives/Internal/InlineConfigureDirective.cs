namespace Typin.Directives.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Directives;
    using Typin.Directives.Builders;

    /// <summary>
    /// Inline command configuration proxy.
    /// </summary>
    /// <typeparam name="TDirective"></typeparam>
    internal sealed class InlineConfigureDirective<TDirective> : IConfigureDirective<TDirective>
        where TDirective : class, IDirective
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, IDirectiveBuilder<TDirective>, CancellationToken, ValueTask> _configure;

        /// <summary>
        /// Initializes a new instance of <see cref="InlineConfigureDirective{TDirective}"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configure"></param>
        public InlineConfigureDirective(IServiceProvider serviceProvider,
                                        Func<IServiceProvider, IDirectiveBuilder<TDirective>, CancellationToken, ValueTask> configure)
        {
            _serviceProvider = serviceProvider;
            _configure = configure;
        }

        /// <inheritdoc/>
        public async ValueTask ConfigureAsync(IDirectiveBuilder<TDirective> builder, CancellationToken cancellationToken)
        {
            await _configure(_serviceProvider, builder, cancellationToken);
        }
    }
}
