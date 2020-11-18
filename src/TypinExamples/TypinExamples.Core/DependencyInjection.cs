namespace TypinExamples.Core
{
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Core.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
        {
            services.AddTransient<WebExampleInvokerService>();

            return services;
        }
    }
}
