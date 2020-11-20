namespace TypinExamples.Core
{
    using System.Reflection;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class WorkerDependencyInjection
    {
        public static IServiceCollection ConfigureWorkerCoreServices(this IServiceCollection services)
        {
            services.AddMediatR(new Assembly[]
            {
                typeof(DependencyInjection).GetTypeInfo().Assembly,
            });

            //services.AddTransient<WebExampleInvokerService>();

            return services;
        }
    }
}
