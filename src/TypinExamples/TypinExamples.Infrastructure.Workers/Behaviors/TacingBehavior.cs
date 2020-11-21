namespace TypinExamples.Infrastructure.Workers.Behaviors
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models;

    public class TracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Trace.WriteLine("Before");

            if (request is IWorkerRequest wr)
            {

            }

            var response = await next();
            Trace.WriteLine("After");

            if (response is WorkerMessageModel message)
            {

            }

            return response;
        }
    }
}
