namespace TypinExamples.Timer
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;
    using TypinExamples.Timer.Middleware;
    using TypinExamples.TypinWeb.Commands;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddCommand<PipelineCommand>()
                                                    .AddCommand<ServicesCommand>()
                                                    .AddDirective<PreviewDirective>()
                                                    .UseMiddleware<ExecutionTimingMiddleware>()
                                                    .UseInteractiveMode()
                                                    .Build()
                                                    .RunAsync();
        }
    }
}
