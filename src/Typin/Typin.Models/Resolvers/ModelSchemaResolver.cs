namespace Typin.Models.Internal
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Models.Builders;
    using Typin.Models.Resolvers;
    using Typin.Models.Schemas;

    /// <summary>
    /// Model schema resolver.
    /// </summary>
    internal sealed class ModelSchemaResolver<TModel> : IModelSchemaResolver<TModel>
        where TModel : class, IModel
    {
        private readonly IEnumerable<IConfigureModel<TModel>> _configurators;

        /// <summary>
        /// Initializes a new instance of <see cref="ModelSchemaProvider"/>.
        /// </summary>
        public ModelSchemaResolver(IEnumerable<IConfigureModel<TModel>> configurators)
        {
            _configurators = configurators;
        }

        /// <inheritdoc/>
        public async Task<IModelSchema> ResolveAsync(CancellationToken cancellationToken = default)
        {
            IModelBuilder<TModel> builder = new ModelBuilder<TModel>();

            foreach (IConfigureModel<TModel> configurator in _configurators)
            {
                await configurator.ConfigureAsync(builder, cancellationToken);
            }

            IModelSchema schema = builder.Build();

            return schema;
        }
    }
}
