namespace TypinExamples.Infrastructure.TypinWeb
{
    using System.Reflection;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructureTypinWebServices(this IServiceCollection services)
        {
            services.AddMediatR(new Assembly[]
            {
                typeof(DependencyInjection).GetTypeInfo().Assembly,
                typeof(Application.DependencyInjection).GetTypeInfo().Assembly
            });

            return services;
        }
    }
}
