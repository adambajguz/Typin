namespace TypinExamples.Core.Handlers.Core.Commands
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
            private readonly IWorkerTaskDispatcher _taskDispatcher;

            public PingHandler(IWorkerTaskDispatcher taskDispatcher)
            {
                _taskDispatcher = taskDispatcher;
            }

            public async Task<string> Handle(PingCommand request, CancellationToken cancellationToken)
            {
                WorkerMessageModel message = this.CreateMessageBuilder()
                                                 .CallCommand(request)
                                                 .Build();

                WorkerMessageModel result = await _taskDispatcher.DispachAsync(message);

                return JsonConvert.SerializeObject(result);
            }
        }

        public class WorkerPingHandler : IWorkerRequestHandler<PingCommand>
        {
            public WorkerPingHandler()
            {

            }

            public Task<WorkerMessageModel> Handle(PingCommand request, CancellationToken cancellationToken)
            {
                WorkerMessageModel message = this.CreateMessageBuilder()
                                                 .CallCommand<PingCommand>(request)
                                                 .AddArgument("Result", $"Processed by WorkerPingCommand {request.Value}")
                                                 .Build();

                var wait = DateTime.UtcNow.AddSeconds(5);

                while (DateTime.UtcNow < wait)
                {

                }

                return Task.FromResult(message);
            }
        }
    }
}
