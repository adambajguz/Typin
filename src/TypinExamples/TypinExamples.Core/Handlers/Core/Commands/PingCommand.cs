namespace TypinExamples.Core.Handlers.Core.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TypinExamples.Core.Handlers.Core;
    using TypinExamples.Core.Handlers.Workers.Commands;
    using TypinExamples.Core.Models;
    using TypinExamples.Core.Services;

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
                WorkerMessageModel result = await _taskDispatcher.RunAsync(new WorkerMessageModel
                {
                    Command = typeof(WorkerPingCommand).AssemblyQualifiedName,
                    Arguments = new()
                    {
                        { nameof(Value), request.Value }
                    }
                });

                return result.Arguments?["Result"] as string ?? string.Empty;
            }
        }
    }
}
