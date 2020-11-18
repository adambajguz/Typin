namespace TypinExamples.Core
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Common.Extensions;
    using TypinExamples.Core.Configuration;
    using TypinExamples.Core.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<ExamplesSettings>(configuration);

            services.AddTransient<WebExampleInvokerService>();

            return services;
        }
    }
}
