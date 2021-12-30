namespace Typin.Directives.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Directives;
    using Typin.Directives.Builders;

    /// <summary>
    /// Inline global directive configuration proxy.
    /// </summary>
    internal sealed class InlineConfigureDirectives : IConfigureDirective
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, IDirectiveBuilder, CancellationToken, ValueTask> _configure;

        /// <summary>
        /// Initializes a new instance of <see cref="InlineConfigureDirectives"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configure"></param>
        public InlineConfigureDirectives(IServiceProvider serviceProvider,
                                         Func<IServiceProvider, IDirectiveBuilder, CancellationToken, ValueTask> configure)
        {
            _serviceProvider = serviceProvider;
            _configure = configure;
        }

        /// <inheritdoc/>
        public async ValueTask ConfigureAsync(IDirectiveBuilder builder, CancellationToken cancellationToken)
        {
            await _configure(_serviceProvider, builder, cancellationToken);
        }
    }
}
