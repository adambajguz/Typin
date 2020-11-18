namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Internal.Extensions;

    internal sealed class ExecuteDirectivesSubpipeline : IMiddleware
    {
        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            IReadOnlyList<IPipelinedDirective> pipelinedDirectives = context.PipelinedDirectives;
            CommandPipelineHandlerDelegate newNext = next;

            foreach (IPipelinedDirective instance in pipelinedDirectives.Reverse())
            {
                newNext = instance.Next(context, newNext, cancellationToken);
            }

            await newNext();
        }
    }
}
