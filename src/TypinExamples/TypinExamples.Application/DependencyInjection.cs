namespace TypinExamples.Core
{
    using System.Reflection;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Services;
    using TypinExamples.Common.Extensions;
    using TypinExamples.Core.Configuration;
    using TypinExamples.Core.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<ExamplesSettings>(configuration);

            services.AddMediatR(new Assembly[]
            {
                typeof(DependencyInjection).GetTypeInfo().Assembly,
            });

            services.AddTransient<WebExampleInvokerService>();
            services.AddTransient<TimerService>();

            return services;
        }
    }
}
