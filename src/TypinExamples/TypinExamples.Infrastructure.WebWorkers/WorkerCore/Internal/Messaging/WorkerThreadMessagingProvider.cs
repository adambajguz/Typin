namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal.Messaging
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal.JS;

    /// <summary>
    /// Simple static message service that runs in the worker thread.
    /// </summary>
    internal sealed class WorkerThreadMessagingProvider : IMessagingProvider
    {
        private static readonly DOMObject _self = new DOMObject("self");

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static event EventHandler<string> _callbacks;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public event EventHandler<string> Callbacks
        {
            add => _callbacks += value;
            remove => _callbacks -= value;
        }

        public WorkerThreadMessagingProvider()
        {

        }

        public void OnMessage(string rawMessage)
        {
            InternalOnMessage(rawMessage);
        }

        internal static void InternalOnMessage(string rawMessage)
        {
#if DEBUG
            Console.WriteLine($"{nameof(WorkerThreadMessagingProvider)}.{nameof(OnMessage)}({rawMessage})");
#endif

            _callbacks?.Invoke(null, rawMessage);
        }

        public Task PostAsync(ulong? workerId, string rawMessage)
        {
#if DEBUG
            Console.WriteLine($"{nameof(WorkerThreadMessagingProvider)}.{nameof(PostAsync)}({workerId}, {rawMessage})");
#endif
            if (workerId is not null)
                throw new ArgumentException("Id must be not null. Cross-worker communication is not supported.", nameof(workerId));

            _self.Invoke("postMessage", rawMessage);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //TODO: dispose
            //_self.Dispose();
        }
    }
}
