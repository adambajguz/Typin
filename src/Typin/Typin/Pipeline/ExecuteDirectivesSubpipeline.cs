namespace Typin.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    /// <summary>
    /// Executes directives subpipeline.
    /// </summary>
    public sealed class ExecuteDirectivesSubpipeline : IMiddleware
    {
        private readonly IStepActivator _stepActivator;

        /// <summary>
        /// Initializes a new instance of <see cref="ExecuteDirectivesSubpipeline"/>.
        /// </summary>
        public ExecuteDirectivesSubpipeline(IStepActivator stepActivator)
        {
            _stepActivator = stepActivator;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<IPipelinedDirective> pipelinedDirectives = args.PipelinedDirectives ?? throw new NullReferenceException($"{nameof(CliContext.PipelinedDirectives)} must be set in {nameof(CliContext)}.");

            if (pipelinedDirectives.Count > 0)
            {
                IPipelineBuilder<CliContext> builder = PipelineBuilder.Create<CliContext>()
                                    .Description(args.Id.ToString())
                                    .Lifetime(InvokablePipelineLifetime.Transient);

                builder.AddSteps(pipelinedDirectives);
                IPipeline<CliContext> pipeline = builder.Build();

                IInvokablePipeline<CliContext> invokableSubPipeline = pipeline.CreateInvokable(_stepActivator);
                await invokableSubPipeline.InvokeAsync(args, next, cancellationToken);
            }
            else
            {
                await next();
            }
        }
    }
}
