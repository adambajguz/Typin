namespace TypinExamples.Timer
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;
    using TypinExamples.Infrastructure.TypinWeb.Commands;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;
    using TypinExamples.Timer.Middleware;
    using TypinExamples.Timer.Repositories;
    using TypinExamples.TypinWeb.Extensions;

    public static class WebProgram
    {
        public static async Task<int> WebMain(WebCliConfiguration configuration, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddCommand<PipelineCommand>()
                                                    .AddCommand<ServicesCommand>()
                                                    .AddDirectivesFromThisAssembly()
                                                    .AddDirective<PreviewDirective>()
                                                    .UseMiddleware<ExecutionTimingMiddleware>()
                                                    .ConfigureServices((services) => services.AddSingleton<IPerformanceLogsRepository, PerformanceLogsRepository>())
                                                    .UseInteractiveMode(options: (cfg) => cfg.IsAdvancedInputAvailable = false)
                                                    .UseWebExample(configuration)
                                                    .Build()
                                                    .RunAsync(commandLine, environmentVariables, true);
        }
    }
}
