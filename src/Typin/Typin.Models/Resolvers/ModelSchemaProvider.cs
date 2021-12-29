namespace Typin.Models.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting.Components;
    using Typin.Models.Collections;
    using Typin.Models.Resolvers;
    using Typin.Models.Schemas;

    /// <summary>
    /// Default implementation of <see cref="IModelSchemaProvider"/>
    /// </summary>
    internal sealed class ModelSchemaProvider : IModelSchemaProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IComponents<IModel> _models;

        /// <inheritdoc/>
        public IModelSchemaCollection Schemas { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ModelSchemaProvider"/>.
        /// </summary>
        public ModelSchemaProvider(IServiceProvider serviceProvider,
                                   IModelSchemaCollection schemas,
                                   IComponents<IModel> models)
        {
            _serviceProvider = serviceProvider;
            _models = models;

            Schemas = schemas;
        }

        /// <inheritdoc/>
        public async Task ReloadAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Type>? modelsTypes = _models.Types;

            if (modelsTypes is null or { Count: 0 })
            {
                return;
            }

            foreach (Type type in modelsTypes)
            {
                Type modelSchemaResolverType = typeof(IModelSchemaResolver<>).MakeGenericType(type);

                IModelSchemaResolver modelSchemaResolver = (IModelSchemaResolver)_serviceProvider.GetRequiredService(modelSchemaResolverType);
                IModelSchema schema = await modelSchemaResolver.ResolveAsync(cancellationToken);

                Schemas.Set(schema);
            }
        }
    }
}
