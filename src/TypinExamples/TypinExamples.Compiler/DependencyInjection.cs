namespace TypinExamples.Compiler.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureCompilerServices(this IServiceCollection services)
        {
            services.AddScoped<RoslynCompilerService>();

            return services;
        }
    }
}
