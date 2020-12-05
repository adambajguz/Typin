namespace TypinExamples.Shared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;

    public class TestCommand : ICommand
    {
        public int Value { get; init; }

        public class Handler : ICommandHandler<TestCommand>
        {
            public async ValueTask<CommandFinished> HandleAsync(TestCommand request, IWorker worker, CancellationToken cancellationToken)
            {
                await worker.CallCommandAsync(new WorkerTestCommand { Value = "123command" });
                Console.WriteLine(request.Value);

                return CommandFinished.Instance;
            }
        }
    }
}
