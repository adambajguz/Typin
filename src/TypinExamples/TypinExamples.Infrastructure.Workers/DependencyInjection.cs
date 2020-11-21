namespace TypinExamples.Infrastructure.Workers
{
    using BlazorWorker.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Services;
    using TypinExamples.Common.Extensions;
    using TypinExamples.Infrastructure.Workers.Configuration;
    using TypinExamples.Infrastructure.Workers.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureWorkersLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWorkerFactory();

            services.AddConfiguration<WorkersSettings>(configuration);

            services.AddScoped<IWorkerMessageDispatcher, WorkerMessageDispatcher>();

            return services;
        }
    }
}
