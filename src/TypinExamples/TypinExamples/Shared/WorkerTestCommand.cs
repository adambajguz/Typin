namespace TypinExamples.Shared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;

    public class WorkerTestCommand
    {
        public string Value { get; init; }

        public class Handler : ICommandHandler<WorkerTestCommand>
        {
            public ValueTask<CommandFinished> HandleAsync(WorkerTestCommand request, CancellationToken cancellationToken)
            {
                Console.WriteLine(request.Value);

                return CommandFinished.Task;
            }
        }
    }
}
