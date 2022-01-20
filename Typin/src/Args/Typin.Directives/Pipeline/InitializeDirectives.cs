namespace Typin.Directives.Pipeline
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Directives;
    using Typin.Directives.Collections;
    using Typin.Directives.Features;
    using Typin.Directives.Schemas;
    using Typin.Features;
    using Typin.Features.Binding;

    /// <summary>
    /// Initializes directives.
    /// </summary>
    public sealed class InitializeDirectives : IMiddleware
    {
        private readonly ConcurrentDictionary<Type, ObjectFactory> _directiveFactoryCache = new();

        private readonly IDirectiveSchemaCollection _directiveSchemas;

        /// <summary>
        /// Initializes an instance of <see cref="InitializeDirectives"/>.
        /// </summary>
        public InitializeDirectives(IDirectiveSchemaCollection directiveSchemas)
        {
            _directiveSchemas = directiveSchemas;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            IBinderFeature binder = args.Binder;
            IServiceProvider serviceProvider = args.Services;

            IUnboundedDirectiveCollection unboundedDirectives = binder.UnboundedTokens;

            //Initialize collections
            List<DirectiveInstance> instances = new();

            //Process directive input
            foreach (IUnboundedDirectiveToken directiveToken in unboundedDirectives)
            {
                IDirectiveSchema? directiveSchema = _directiveSchemas.Get(directiveToken.Alias);

                if (directiveSchema is not null)
                {
                    IDirective model = GetDirectiveInstance(serviceProvider, directiveSchema);
                    IDirectiveHandler handler = GetDirectiveHandlerInstance(serviceProvider, model, directiveSchema);

                    DirectiveInstance instance = new(directiveToken.Id, model, handler);
                    instances.Add(instance);

                    args.Binder.Add(new BindableModel(directiveToken.Id, directiveSchema.Model, model));
                }
            }

            //Set directives lists in context
            args.Features.Set<IDirectivesFeature>(new DirectivesFeature(instances));

            await next();
        }

        private IDirective GetDirectiveInstance(IServiceProvider serviceProvider, IDirectiveSchema schema)
        {
            ObjectFactory factory = _directiveFactoryCache.GetOrAdd(schema.Model.Type, (key) =>
            {
                return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
            });

            return (IDirective)factory(serviceProvider, null);
        }

        private IDirectiveHandler GetDirectiveHandlerInstance(IServiceProvider serviceProvider, IDirective instance, IDirectiveSchema schema)
        {
            if (schema.Handler == schema.Model.Type)
            {
                return (IDirectiveHandler)instance;
            }

            ObjectFactory factory = _directiveFactoryCache.GetOrAdd(schema.Handler, (key) =>
            {
                return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
            });

            return (IDirectiveHandler)factory(serviceProvider, null);
        }
    }
}
