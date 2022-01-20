namespace Typin.Directives.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.StepActivators;
    using Typin;
    using Typin.Directives;
    using Typin.Directives.Features;

    /// <summary>
    /// Handles directives using a subpipeline.
    /// </summary>
    public sealed class HandleDirectives : IMiddleware
    {
        private readonly IBaseStepActivator _stepActivator;

        /// <summary>
        /// Initializes a new instance of <see cref="HandleDirectives"/>.
        /// </summary>
        public HandleDirectives(IScopedStepActivator stepActivator)
        {
            _stepActivator = stepActivator;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            IDirectivesFeature feature = args.Features.Get<IDirectivesFeature>() ??
                throw new InvalidOperationException($"{nameof(IDirectivesFeature)} has not been configured for this application or call.");

            //args.Binder.Validate();

            IReadOnlyList<DirectiveInstance> instances = feature.Instances;

            if (instances.Count > 0)
            {
                int current = 0;

                StepDelegate stepDelegate = null!;
                stepDelegate = async () =>
                {
                    if (current < instances.Count)
                    {
                        DirectiveInstance instance = instances[current++];

                        DirectiveArgs directiveArgs = new(instance, args);
                        await instance.Handler.ExecuteAsync(directiveArgs, stepDelegate, cancellationToken);
                    }
                    else
                    {
                        await next();
                    }
                };

                await stepDelegate();
            }
            else
            {
                await next();
            }
        }
    }
}
