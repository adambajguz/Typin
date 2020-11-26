namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IWorkerStartup
    {
        void Configure(IWorkerConfigurationBuilder builder);
        void ConfigureServices(IServiceCollection services);
    }
}
