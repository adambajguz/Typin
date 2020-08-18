using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Typin.Demo.Services;
using Typin.Directives;

namespace Typin.Demo
{
    public static class Program
    {
        private static void ConfigureServices(IServiceCollection services)
        {
            // Register services
            services.AddSingleton<LibraryService>();
        }

        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder()
                .ConfigureServices(ConfigureServices)
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .Build()
                .RunAsync();
        }
    }
}