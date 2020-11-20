namespace TypinExamples.Core.Handlers.Workers.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Core.Models;

    public class WorkerPingCommand : IWorkerRequest
    {
        public int Value { get; init; }

        public class WorkerPingHandler : IWorkerRequestHandler<WorkerPingCommand>
        {
            public WorkerPingHandler()
            {

            }

            public Task<WorkerMessageModel> Handle(WorkerPingCommand request, CancellationToken cancellationToken)
            {
                //return $"This is a string from worker {request.Value}";
                return Task.FromResult(new WorkerMessageModel());
            }
        }
    }
}
