namespace TypinExamples.Shared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public class TestNotification
    {
        public int Value { get; init; }

        public class Handler : INotificationHandler<TestNotification>
        {
            private readonly IMessagingService _messaging;

            public Handler(IMessagingService messagingService)
            {
                _messaging = messagingService;
            }

            public ValueTask HandleAsync(TestNotification request, CancellationToken cancellationToken)
            {
                Console.WriteLine(request.Value);

                return default;
            }
        }
    }
}
