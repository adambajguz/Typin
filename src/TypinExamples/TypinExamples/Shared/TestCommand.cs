namespace TypinExamples.Shared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Payloads;

    public class TestCommand
    {
        public int Value { get; init; }

        public class Handler : ICommandHandler<TestCommand>
        {
            private readonly IMessagingService _messaging;

            public Handler(IMessagingService messagingService)
            {
                _messaging = messagingService;
            }

            public ValueTask<CommandFinished> HandleAsync(TestCommand request, CancellationToken cancellationToken)
            {
                Console.WriteLine(request.Value);

                return CommandFinished.Task;
            }
        }
    }
}
