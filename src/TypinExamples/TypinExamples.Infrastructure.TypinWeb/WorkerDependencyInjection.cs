namespace TypinExamples.Infrastructure.TypinWeb
{
    using System.Reflection;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Services;
    using TypinExamples.Infrastructure.TypinWeb.Services;

    public static class WorkerDependencyInjection
    {
        public static IServiceCollection ConfigureWorkerCoreServices(this IServiceCollection services)
        {
            services.AddMediatR(new Assembly[]
            {
                typeof(DependencyInjection).GetTypeInfo().Assembly,
            });

            services.AddTransient<TimerService>()
                    .AddTransient<IWebExampleInvokerService, WebExampleInvokerService>();

            return services;
        }
    }
}
