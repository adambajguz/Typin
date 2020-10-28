namespace TypinExamples.Timer.Middleware
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;

    public class TimingMiddleware : IMiddleware
    {
        public Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
