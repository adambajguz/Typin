namespace TypinExamples.Validation
{
    using System.Threading.Tasks;
    using Typin;

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
