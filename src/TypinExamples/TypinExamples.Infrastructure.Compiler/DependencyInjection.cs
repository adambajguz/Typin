namespace TypinExamples.Infrastructure.Compiler
{
    using Microsoft.Extensions.DependencyInjection;
    using TypinExamples.Infrastructure.Compiler.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructureCompilerServices(this IServiceCollection services)
        {
            services.AddScoped<RoslynCompilerService>();

            return services;
        }
    }
}
