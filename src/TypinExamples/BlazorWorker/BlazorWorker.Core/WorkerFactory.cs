namespace BlazorWorker.Core
{
    using System.Threading.Tasks;
    using Microsoft.JSInterop;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class WorkerFactory : IWorkerFactory
    {
        private readonly IJSRuntime jsRuntime;

        public WorkerFactory(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IWorker> CreateAsync()//WorkerInitOptions initOptions)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var worker = new WorkerProxy(jsRuntime);
            //await worker.InitAsync(initOptions);
            return worker;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
