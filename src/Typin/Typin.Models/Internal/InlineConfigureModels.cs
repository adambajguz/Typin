namespace Typin.Models.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Models;
    using Typin.Models.Builders;

    /// <summary>
    /// Inline global model configuration proxy.
    /// </summary>
    internal sealed class InlineConfigureModels : IConfigureModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, IModelBuilder, CancellationToken, ValueTask> _configure;

        /// <summary>
        /// Initializes a new instance of <see cref="InlineConfigureModels"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configure"></param>
        public InlineConfigureModels(IServiceProvider serviceProvider,
                                     Func<IServiceProvider, IModelBuilder, CancellationToken, ValueTask> configure)
        {
            _serviceProvider = serviceProvider;
            _configure = configure;
        }

        /// <inheritdoc/>
        public async ValueTask ConfigureAsync(IModelBuilder builder, CancellationToken cancellationToken)
        {
            await _configure(_serviceProvider, builder, cancellationToken);
        }
    }
}
