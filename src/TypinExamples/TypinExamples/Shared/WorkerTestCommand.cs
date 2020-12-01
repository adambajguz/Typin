namespace TypinExamples.Shared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;

    public class WorkerTestCommand
    {
        public string? Value { get; init; }

        public class Handler : ICommandHandler<WorkerTestCommand>
        {
            public ValueTask<CommandFinished> HandleAsync(WorkerTestCommand request, IWorker worker, CancellationToken cancellationToken)
            {
                Console.WriteLine(request.Value ?? string.Empty);

                return CommandFinished.Task;
            }
        }
    }
}
