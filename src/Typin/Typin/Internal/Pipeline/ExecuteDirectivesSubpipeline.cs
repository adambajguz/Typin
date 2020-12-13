namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Typin;
    using Typin.Internal.Extensions;

    internal sealed class ExecuteDirectivesSubpipeline : IMiddleware
    {
        private readonly ILogger _logger;

        public ExecuteDirectivesSubpipeline(ILogger<ExecuteDirectivesSubpipeline> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            IReadOnlyList<IPipelinedDirective> pipelinedDirectives = context.PipelinedDirectives;
            CommandPipelineHandlerDelegate newNext = next;

            foreach (IPipelinedDirective instance in pipelinedDirectives.Reverse())
            {
                newNext = instance.Next(context, newNext, _logger, cancellationToken);
            }

            await newNext();
        }
    }
}
