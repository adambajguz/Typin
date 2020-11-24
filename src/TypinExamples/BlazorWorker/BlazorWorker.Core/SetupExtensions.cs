namespace BlazorWorker.Core
{
    using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class SetupExtensions
    {
        /// <summary>
        /// Adds <see cref="IWorkerFactory"/> as a singleton service
        /// to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddWorkerFactory(this IServiceCollection services)
        {
            services.AddSingleton<IWorkerFactory, WorkerFactory>();
            return services;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
