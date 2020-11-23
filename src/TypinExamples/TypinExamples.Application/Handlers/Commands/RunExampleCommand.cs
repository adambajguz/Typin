namespace TypinExamples.Application.Handlers.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using TypinExamples.Application.Services.Workers;
    using TypinExamples.Domain.Extensions;
    using TypinExamples.Domain.Interfaces.Handlers.Core;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models.Workers;

    public class RunExampleCommand : ICoreRequest<string>, IWorkerRequest
    {
        public long? WorkerId { get; set; }

        public int Value { get; init; }

        public class RunExampleHandler : ICoreRequestHandler<RunExampleCommand, string>
        {
            private readonly IWorkerMessageDispatcher _taskDispatcher;

            public RunExampleHandler(IWorkerMessageDispatcher taskDispatcher)
            {
                _taskDispatcher = taskDispatcher;
            }

            public async Task<string> Handle(RunExampleCommand request, CancellationToken cancellationToken)
            {
                WorkerMessage message = this.CreateMessageBuilder()
                                            .CallCommand(request)
                                            .Build();

                WorkerResult result = await _taskDispatcher.DispachAsync(message);

                return JsonConvert.SerializeObject(result);
            }
        }
    }
}
