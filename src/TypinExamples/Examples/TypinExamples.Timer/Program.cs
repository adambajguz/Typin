namespace TypinExamples.Timer
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;
    using TypinExamples.Timer.Middleware;
    using TypinExamples.Timer.Repositories;
    using TypinExamples.TypinWeb.Commands;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddCommand<PipelineCommand>()
                                                    .AddCommand<ServicesCommand>()
                                                    .AddDirectivesFromThisAssembly()
                                                    .AddDirective<PreviewDirective>()
                                                    .UseMiddleware<ExecutionTimingMiddleware>()
                                                    .ConfigureServices((services) => services.AddSingleton<IPerformanceLogsRepository, PerformanceLogsRepository>())
                                                    .UseInteractiveMode()
                                                    .Build()
                                                    .RunAsync();
        }
    }
}
