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
    internal sealed class InlineConfigureModel : IConfigureModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, IModelBuilder, CancellationToken, ValueTask> _configure;

        /// <summary>
        /// Initializes a new instance of <see cref="InlineConfigureModel"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configure"></param>
        public InlineConfigureModel(IServiceProvider serviceProvider,
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

    /// <summary>
    /// Inline model configuration proxy.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal sealed class InlineConfigureModel<TModel> : IConfigureModel<TModel>
        where TModel : class, IModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, IModelBuilder<TModel>, CancellationToken, ValueTask> _configure;

        /// <summary>
        /// Initializes a new instance of <see cref="InlineConfigureModel{TModel}"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configure"></param>
        public InlineConfigureModel(IServiceProvider serviceProvider,
                                    Func<IServiceProvider, IModelBuilder<TModel>, CancellationToken, ValueTask> configure)
        {
            _serviceProvider = serviceProvider;
            _configure = configure;
        }

        /// <inheritdoc/>
        public async ValueTask ConfigureAsync(IModelBuilder<TModel> builder, CancellationToken cancellationToken)
        {
            await _configure(_serviceProvider, builder, cancellationToken);
        }
    }
}
