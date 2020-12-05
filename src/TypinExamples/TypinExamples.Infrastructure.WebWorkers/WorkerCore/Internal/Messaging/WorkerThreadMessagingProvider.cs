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
        private static event EventHandler<string> _callbacks = default!;

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

        public Task PostAsync(ulong? targetWorkerId, string rawMessage)
        {
#if DEBUG
            Console.WriteLine($"{nameof(WorkerThreadMessagingProvider)}.{nameof(PostAsync)}({targetWorkerId}, {rawMessage})");
#endif
            if (targetWorkerId is not null)
                throw new ArgumentException("Id must be null. Cross-worker communication is not supported.", nameof(targetWorkerId));

            _self.Invoke("postMessage", rawMessage);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _callbacks = default!;
            //TODO: dispose
            //_self.Dispose();
        }
    }
}
