namespace TypinExamples.Timer
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Directives;
    using TypinExamples.Timer.Middleware;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddDirective<PreviewDirective>()
                                                    .UseMiddleware<ExecutionTimingMiddleware>()
                                                    .Build()
                                                    .RunAsync();
        }
    }
}
