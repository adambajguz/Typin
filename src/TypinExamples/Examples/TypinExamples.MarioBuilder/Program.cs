namespace TypinExamples.MarioBuilder
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Directives;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddDirective<PreviewDirective>()
                                                    .Build()
                                                    .RunAsync();
        }
    }
}
