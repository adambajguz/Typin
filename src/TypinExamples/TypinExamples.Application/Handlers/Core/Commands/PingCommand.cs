namespace TypinExamples.Core.Handlers.Core.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using TypinExamples.Core.Handlers.Workers.Commands;
    using TypinExamples.Core.Services;
    using TypinExamples.Domain.Extensions;
    using TypinExamples.Domain.Interfaces.Handlers.Core;
    using TypinExamples.Domain.Models;

    public class PingCommand : ICoreRequest<string>
    {
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
                                                 .CallCommand<WorkerPingCommand>()
                                                 .AddArgument(nameof(Value), request.Value)
                                                 .Build();

                WorkerMessageModel result = await _taskDispatcher.RunAsync(message);

                return JsonConvert.SerializeObject(result);
            }
        }
    }
}
