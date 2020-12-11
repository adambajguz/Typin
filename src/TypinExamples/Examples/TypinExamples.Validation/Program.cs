namespace TypinExamples.Validation
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Directives;
    using Typin.Modes;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().UseStartup<Startup>()
                                                    .Build()
                                                    .RunAsync();
        }
    }
}
