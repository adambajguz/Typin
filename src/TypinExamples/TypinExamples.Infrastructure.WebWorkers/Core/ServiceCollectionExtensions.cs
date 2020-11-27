namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="IWorkerFactory"/> as a singleton service
        /// to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddWebWorkers(this IServiceCollection services)
        {
            services.AddScoped<IWorkerFactory, WorkerFactory>();

            return services;
        }
    }
}
