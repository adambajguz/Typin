namespace BlazorWorker.BackgroundServiceFactory
{
    using System;
    using System.Threading.Tasks;
    using BlazorWorker.Core;
    using BlazorWorker.WorkerBackgroundService;

    public static class WorkerBackgroundServiceExtensions
    {
        public static async Task<IWorkerBackgroundService<T>> CreateBackgroundServiceAsync<T>(this IWorker webWorkerProxy, Action<WorkerInitOptions> workerInitOptionsModifier) where T : class
        {
            var options = new WorkerInitOptions();
            workerInitOptionsModifier(options);
            return await webWorkerProxy.CreateBackgroundServiceAsync<T>(options);
        }

        public static async Task<IWorkerBackgroundService<T>> CreateBackgroundServiceAsync<T>(this IWorker webWorkerProxy, WorkerInitOptions workerInitOptions = null) where T : class
        {
            var proxy = new WorkerBackgroundServiceProxy<T>(webWorkerProxy, new WebWorkerOptions());
            if (workerInitOptions == null)
            {
                workerInitOptions = new WorkerInitOptions().AddAssemblyOf<T>();
            }

            await proxy.InitAsync(workerInitOptions);
            return proxy;
        }
    }
}

