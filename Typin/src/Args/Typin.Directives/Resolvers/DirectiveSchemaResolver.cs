namespace Typin.Directives.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Directives.Builders;
    using Typin.Directives.Schemas;
    using Typin.Models.Collections;
    using Typin.Models.Schemas;

    /// <summary>
    /// Directive schema resolver.
    /// </summary>
    internal sealed class DirectiveSchemaResolver<TDirective> : IDirectiveSchemaResolver<TDirective>
        where TDirective : class, IDirective
    {
        private readonly IModelSchemaCollection _modelSchemas;
        private readonly IEnumerable<IConfigureDirective> _globalConfigurators;
        private readonly IEnumerable<IConfigureDirective<TDirective>> _configurators;

        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveSchemaProvider"/>.
        /// </summary>
        public DirectiveSchemaResolver(IEnumerable<IConfigureDirective> globalConfigurators,
                                       IEnumerable<IConfigureDirective<TDirective>> configurators,
                                       IModelSchemaCollection modelSchemas)
        {
            _globalConfigurators = globalConfigurators;
            _configurators = configurators;
            _modelSchemas = modelSchemas;
        }

        /// <inheritdoc/>
        public async Task<IDirectiveSchema> ResolveAsync(CancellationToken cancellationToken = default)
        {
            IModelSchema directiveModelSchema = _modelSchemas[typeof(TDirective)] ??
                throw new InvalidOperationException($"Cannot resolve '{typeof(IDirectiveSchema)}' for '{typeof(TDirective)}' because its '{nameof(IModelSchema)}' was not found.");

            DirectiveBuilder<TDirective> builder = new(directiveModelSchema);

            foreach (IConfigureDirective globalConfigurator in _globalConfigurators)
            {
                await globalConfigurator.ConfigureAsync(builder, cancellationToken);
            }

            foreach (IConfigureDirective<TDirective> configurator in _configurators)
            {
                await configurator.ConfigureAsync(builder, cancellationToken);
            }

            IDirectiveSchema schema = builder.Build();

            return schema;
        }
    }
}
