namespace TypinExamples.Application
{
    using System.Reflection;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Application.Services;

    public static class WorkerDependencyInjection
    {
        public static IServiceCollection ConfigureWorkerCoreServices(this IServiceCollection services)
        {
            services.AddMediatR(new Assembly[]
            {
                typeof(DependencyInjection).GetTypeInfo().Assembly,
            });

            services.AddTransient<TimerService>();

            //services.AddTransient<WebExampleInvokerService>();

            return services;
        }
    }
}
