namespace Typin.Directives.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Directives.Collections;
    using Typin.Directives.Schemas;
    using Typin.Hosting.Components;

    /// <summary>
    /// Default implementation of <see cref="IDirectiveSchemaProvider"/>
    /// </summary>
    internal sealed class DirectiveSchemaProvider : IDirectiveSchemaProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IComponents<IDirective> _directives;

        /// <inheritdoc/>
        public IDirectiveSchemaCollection Schemas { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveSchemaProvider"/>.
        /// </summary>
        public DirectiveSchemaProvider(IServiceProvider serviceProvider,
                                     IDirectiveSchemaCollection schemas,
                                     IComponents<IDirective> directives)
        {
            _serviceProvider = serviceProvider;
            _directives = directives;

            Schemas = schemas;
        }

        /// <inheritdoc/>
        public async Task ReloadAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Type>? DirectivesTypes = _directives.Types;

            if (DirectivesTypes is null or { Count: 0 })
            {
                return;
            }

            foreach (Type type in DirectivesTypes)
            {
                Type directiveSchemaResolverType = typeof(IDirectiveSchemaResolver<>).MakeGenericType(type);

                IDirectiveSchemaResolver directiveSchemaResolver = (IDirectiveSchemaResolver)_serviceProvider.GetRequiredService(directiveSchemaResolverType);
                IDirectiveSchema schema = await directiveSchemaResolver.ResolveAsync(cancellationToken);

                Schemas.Set(schema); //TODO: better valdiation, e.g. name duplicates
            }
        }
    }
}
