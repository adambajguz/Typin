namespace TypinExamples.Application.Handlers.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using TypinExamples.Core.Services;
    using TypinExamples.Domain.Extensions;
    using TypinExamples.Domain.Interfaces.Handlers.Core;
    using TypinExamples.Domain.Interfaces.Handlers.Workers;
    using TypinExamples.Domain.Models;

    public class PingCommand : ICoreRequest<string>, IWorkerRequest
    {
        public long? WorkerId { get; set; }

        public int Value { get; init; }

        public class PingHandler : ICoreRequestHandler<PingCommand, string>
        {
            private readonly IWorkerMessageDispatcher _taskDispatcher;

            public PingHandler(IWorkerMessageDispatcher taskDispatcher)
            {
                _taskDispatcher = taskDispatcher;
            }

            public async Task<string> Handle(PingCommand request, CancellationToken cancellationToken)
            {
                WorkerMessage message = this.CreateMessageBuilder()
                                            .CallCommand(request)
                                            .Build();

                WorkerResult result = await _taskDispatcher.DispachAsync(message);

                return JsonConvert.SerializeObject(result);
            }
        }

        public class WorkerPingHandler : IWorkerRequestHandler<PingCommand>
        {
            public WorkerPingHandler()
            {

            }

            public Task<WorkerResult> Handle(PingCommand request, CancellationToken cancellationToken)
            {
                var wait = DateTime.UtcNow.AddSeconds(5);

                while (DateTime.UtcNow < wait)
                {

                }

                return Task.FromResult(new WorkerResult { Data = $"Processed by WorkerPingHandler {request.Value}" });
            }
        }
    }
}
