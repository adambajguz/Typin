namespace TypinExamples.Timer
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;
    using TypinExamples.Infrastructure.TypinWeb.Commands;
    using TypinExamples.Timer.Middleware;
    using TypinExamples.Timer.Repositories;

    public class Startup : ICliStartup
    {
        public void Configure(CliApplicationBuilder app)
        {
            app.AddCommandsFromThisAssembly()
               .AddCommand<PipelineCommand>()
               .AddCommand<ServicesCommand>()
               .AddDirectivesFromThisAssembly()
               .AddDirective<PreviewDirective>()
               .UseMiddleware<ExecutionTimingMiddleware>()
               .UseInteractiveMode(options: (cfg) => cfg.IsAdvancedInputAvailable = false);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPerformanceLogsRepository, PerformanceLogsRepository>();
        }
    }
}
