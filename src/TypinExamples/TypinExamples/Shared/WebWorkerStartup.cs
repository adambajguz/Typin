namespace TypinExamples.Shared
{
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebWorkerStartup : IWorkerStartup
    {
        public void Configure(IWorkerConfigurationBuilder builder)
        {
            builder.UseProgram<WebWorkerProgram>();
        }

        public void ConfigureServices(IServiceCollection services)
        {

        }
    }
}
