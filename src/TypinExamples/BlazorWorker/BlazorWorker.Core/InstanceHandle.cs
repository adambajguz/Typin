namespace BlazorWorker.Core
{
    using System;
    using BlazorWorker.WorkerCore;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public sealed class InstanceHandle : IDisposable
    {
        public InstanceHandle(
            IWorkerMessageService messageService,
            Type serviceType,
            long identifier,
            Action onDispose)
        {
            MessageService = messageService;
            ServiceType = serviceType;
            Identifier = identifier;
            OnDispose = onDispose;
        }

        public IWorkerMessageService MessageService { get; }

        public Type ServiceType { get; }
        public long Identifier { get; }
        public Action OnDispose { get; }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
