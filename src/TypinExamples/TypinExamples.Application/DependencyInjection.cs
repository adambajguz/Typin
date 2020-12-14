namespace TypinExamples.Application
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Configurations;
    using TypinExamples.Application.Services;
    using TypinExamples.Common.Extensions;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<ExamplesConfiguration>(configuration);

            services.AddTransient<TimerService>();

            return services;
        }
    }
}
