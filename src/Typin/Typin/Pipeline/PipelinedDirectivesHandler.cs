namespace Typin.Pipeline
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    /// <summary>
    /// Handles pipelined directives using a subpipeline.
    /// </summary>
    public sealed class PipelinedDirectivesHandler : IMiddleware
    {
        private readonly IStepActivator _stepActivator;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelinedDirectivesHandler"/>.
        /// </summary>
        public PipelinedDirectivesHandler(IStepActivator stepActivator)
        {
            _stepActivator = stepActivator;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<IPipelinedDirective> pipelinedDirectives = args.Directives.Pipelined;

            if (pipelinedDirectives.Count > 0)
            {
                IPipeline<CliContext> pipeline = PipelineBuilder.Create<CliContext>()
                    .Description(args.Call.Identifier.ToString())
                    .Lifetime(InvokablePipelineLifetime.Transient)
                    .AddSteps(pipelinedDirectives)
                    .Build();

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
