namespace Typin.Examples
{
    using System;
    using System.Net.Http;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Examples.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this WebAssemblyHostBuilder builder)
        {
            IServiceCollection services = builder.Services;

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            services.AddScoped<MonacoEditorService>();

            return services;
        }
    }
}
