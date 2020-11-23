namespace TypinExamples.Infrastructure.TypinWeb.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models.Workers;

    public class WorkerRunExampleHandler : IWorkerRequestHandler<RunExampleCommand>
    {
        public WorkerRunExampleHandler()
        {

        }

        public Task<WorkerResult> Handle(RunExampleCommand request, CancellationToken cancellationToken)
        {
            var wait = DateTime.UtcNow.AddSeconds(5);

            while (DateTime.UtcNow < wait)
            {

            }

            return Task.FromResult(new WorkerResult { Data = $"Processed by WorkerPingHandler {request.Value}" });
        }
    }
}
