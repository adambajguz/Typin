namespace TypinExamples.Application
{
    using System.Reflection;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Configuration;
    using TypinExamples.Application.Services;
    using TypinExamples.Common.Extensions;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<ExamplesSettings>(configuration);

            services.AddMediatR(new Assembly[]
            {
                typeof(DependencyInjection).GetTypeInfo().Assembly,
            });

            services.AddTransient<TimerService>();

            return services;
        }
    }
}
