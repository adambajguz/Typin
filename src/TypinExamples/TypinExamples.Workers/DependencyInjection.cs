namespace TypinExamples.Workers
{
    using BlazorWorker.Core;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Core.Services;
    using TypinExamples.Workers.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureWorkersLayer(this IServiceCollection services)
        {
            services.AddWorkerFactory();

            services.AddScoped<IWorkerTaskDispatcher, WorkerTaskDispatcher>();

            return services;
        }
    }
}
