namespace InteractiveModeExample
{
    using System.Threading.Tasks;
    using InteractiveModeExample.Directives;
    using InteractiveModeExample.Middlewares;
    using InteractiveModeExample.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;

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
               // .AddCommandsFromThisAssembly()
                .AddDirective<DebugDirective>()
                .AddDirective<PreviewDirective>()
                .AddDirective<CustomInteractiveModeOnlyDirective>()
                .UseMiddleware<ExecutionTimingMiddleware>()
                .UseDirectMode(true)
               // .UseInteractiveMode()
                .ConfigureLogging(cfg =>
                {
                    cfg.SetMinimumLevel(LogLevel.Debug);
                })
                .UseStartupMessage("{title} CLI {version} {{title}} {executable} {{{description}}} {test}")
                .Build()
                .RunAsync();
        }
    }
}