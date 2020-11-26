namespace BlazorWorker.Core.CoreInstanceService
{
    using System;
    using System.Threading.Tasks;

    internal class CoreInstanceHandle : IInstanceHandle
    {
        private readonly Func<Task> onDispose;

        public CoreInstanceHandle(Func<Task> onDispose)
        {
            this.onDispose = onDispose;
        }

        public async ValueTask DisposeAsync()
        {
            await onDispose();
        }
    }
}
