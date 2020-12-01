namespace TypinExamples.Infrastructure.TypinWeb
{
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Services;
    using TypinExamples.Infrastructure.TypinWeb.Services;

    public static class WorkerDependencyInjection
    {
        public static IServiceCollection ConfigureWorkerCoreServices(this IServiceCollection services)
        {
            services.AddTransient<TimerService>()
                    .AddTransient<IWebExampleInvokerService, WebExampleInvokerService>();

            return services;
        }
    }
}
