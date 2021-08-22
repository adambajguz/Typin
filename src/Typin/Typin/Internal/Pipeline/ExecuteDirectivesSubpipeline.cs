namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    internal sealed class ExecuteDirectivesSubpipeline : IMiddleware
    {
        private readonly IStepActivator _stepActivator;

        public ExecuteDirectivesSubpipeline(IStepActivator stepActivator)
        {
            _stepActivator = stepActivator;
        }

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<IPipelinedDirective> pipelinedDirectives = args.PipelinedDirectives;

            if (pipelinedDirectives.Count > 0)
            {
                var builder = PipelineBuilder.Create<ICliContext>()
                    .Description(args.Id.ToString())
                    .Lifetime(InvokablePipelineLifetime.Transient);

                foreach (IPipelinedDirective instance in pipelinedDirectives.Reverse())
                {
                    builder.Add(instance);
                }

                IInvokablePipeline<ICliContext> invokableSubPipeline = builder.Build().CreateInvokable(_stepActivator);
                await invokableSubPipeline.InvokeAsync(args, cancellationToken);
                await next();
            }
            else
            {
                await next();
            }
        }
    }
}
