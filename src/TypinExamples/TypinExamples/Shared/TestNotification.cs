namespace TypinExamples.Shared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public class TestNotification : INotification
    {
        public int Value { get; init; }

        public class Handler : INotificationHandler<TestNotification>
        {
            public async ValueTask HandleAsync(TestNotification request, IWorker worker, CancellationToken cancellationToken)
            {
                await worker.NotifyAsync(new WorkerTestNotification { Value = "123notification" });
                Console.WriteLine(request.Value);
            }
        }
    }
}
