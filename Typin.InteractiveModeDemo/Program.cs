using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Typin.Directives;
using Typin.InteractiveModeDemo.Middlewares;
using Typin.InteractiveModeDemo.Services;

namespace Typin.InteractiveModeDemo
{
    public static class Program
    {
        private static void GetServiceCollection(IServiceCollection services)
        {
            services.AddSingleton<LibraryService>();
        }

        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder()
                .ConfigureServices(GetServiceCollection)
                .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .UseMiddleware<ExitCodeMiddleware>()
                .UseMiddleware<ExecutionTimingMiddleware>()
                .UseMiddleware<ExecutionLogMiddleware>()
                .UseInteractiveMode()
                .UseStartupMessage("{title} CLI {version} {{title}} {executable} {{{description}}} {test}")
                .Build()
                .RunAsync();
        }
    }
}