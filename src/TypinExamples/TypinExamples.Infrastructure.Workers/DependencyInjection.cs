namespace TypinExamples.Workers
{
    using BlazorWorker.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Common.Extensions;
    using TypinExamples.Core.Services;
    using TypinExamples.Infrastructure.Workers.Configuration;
    using TypinExamples.Workers.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureWorkersLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWorkerFactory();

            services.AddConfiguration<WorkersSettings>(configuration);

            services.AddScoped<IWorkerTaskDispatcher, WorkerTaskDispatcher>();

            return services;
        }
    }
}
