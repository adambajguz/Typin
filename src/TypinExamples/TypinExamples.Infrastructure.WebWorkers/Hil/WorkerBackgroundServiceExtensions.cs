namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public static class WorkerBackgroundServiceExtensions
    {
        public static async Task<IWorkerBackgroundService<T>> CreateBackgroundServiceAsync<T>(this IWorker webWorkerProxy, Action<WorkerInitOptions> workerInitOptionsModifier) where T : class, IWebWorkerEntryPoint
        {
            var options = new WorkerInitOptions();
            workerInitOptionsModifier(options);
            return await webWorkerProxy.CreateBackgroundServiceAsync<T>(options);
        }

        public static async Task<IWorkerBackgroundService<T>> CreateBackgroundServiceAsync<T>(this IWorker webWorkerProxy, WorkerInitOptions workerInitOptions = null) where T : class, IWebWorkerEntryPoint
        {
            var proxy = new WorkerBackgroundServiceProxy<T>(webWorkerProxy, new WebWorkerOptions());
            if (workerInitOptions == null)
                workerInitOptions = new WorkerInitOptions().AddAssemblyOf<T>();

            await proxy.InitAsync(workerInitOptions);
            return proxy;
        }
    }
}

