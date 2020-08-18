using Microsoft.Extensions.DependencyInjection;
using Typin.BlazorDemo.CLI.Services;
using Typin.Directives;

namespace Typin.BlazorDemo
{
    public class CliStartup : ICliStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register services
            services.AddSingleton<IWebHostRunnerService, WebHostRunnerService>()
                    .AddSingleton<IBackgroundWebHostProvider, BackgroundWebHostProvider>();
        }

        public void Configure(CliApplicationBuilder app)
        {
            app.AddCommandsFromThisAssembly()
               .AddDirective<DebugDirective>()
               .UseInteractiveMode();
        }
    }
}
