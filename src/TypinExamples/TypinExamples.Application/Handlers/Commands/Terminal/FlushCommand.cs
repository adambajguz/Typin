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

    public class FlushCommand : ICoreRequest<string>, IWorkerRequest
    {
        public long? WorkerId { get; set; }

        public string? TerminalId { get; init; }

        public class FlushHandler : ICoreRequestHandler<RunExampleCommand, string>
        {
            private readonly IWorkerMessageDispatcher _taskDispatcher;

            public FlushHandler(IWorkerMessageDispatcher taskDispatcher)
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
