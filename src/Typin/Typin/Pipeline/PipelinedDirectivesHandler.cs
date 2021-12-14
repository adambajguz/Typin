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
                    .Description(args.Call.TraceIdentifier)
                    .Lifetime(InvokablePipelineLifetime.Transient)
                    .AddSteps(pipelinedDirectives)
                    .Build();

                IInvokablePipeline<CliContext> invokableSubPipeline = pipeline.CreateInvokable(_stepActivator);

                try
                {
                    await invokableSubPipeline.InvokeAsync(args, next, cancellationToken);
                }
                catch (PipelineInvocationException ex)
                {
                    throw new System.Exception("Failed to execute pipelined directives.", ex.InnerException); //TODO: replace with custom exception
                }
            }
            else
            {
                await next();
            }
        }
    }
}
