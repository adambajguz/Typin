namespace TypinExamples.Shared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;

    public class WorkerTestNotification
    {
        public string? Value { get; init; }

        public class Handler : INotificationHandler<WorkerTestNotification>
        {
            public ValueTask HandleAsync(WorkerTestNotification request, IWorker worker, CancellationToken cancellationToken)
            {
                Console.WriteLine(request.Value ?? string.Empty);

                return default;
            }
        }
    }
}
