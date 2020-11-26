namespace TypinExamples.Infrastructure.TypinWeb.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models.Workers;

    public class WorkerFlushHandler : IWorkerRequestHandler<FlushCommand>
    {
        public async Task<WorkerResult> Handle(FlushCommand request, CancellationToken cancellationToken)
        {
            System.Console.WriteLine("Test");

            return new WorkerResult { Data = $"Processed by WorkerPingHandler {request.TerminalId}" };
        }
    }
}
