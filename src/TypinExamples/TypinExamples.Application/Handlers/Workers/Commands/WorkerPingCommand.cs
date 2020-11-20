namespace TypinExamples.Core.Handlers.Workers.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Core.Handlers.Core.Commands;
    using TypinExamples.Domain.Extensions;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models;

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
                WorkerMessageModel message = this.CreateMessageBuilder()
                                                 .CallCommand<PingCommand>()
                                                 .AddArgument(nameof(Value), request.Value)
                                                 .Build();

                return Task.FromResult(new WorkerMessageModel());
            }
        }
    }
}
