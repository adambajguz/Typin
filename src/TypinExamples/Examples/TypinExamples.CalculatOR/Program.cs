namespace TypinExamples.CalculatOR
{
    using System.Threading.Tasks;
    using Typin;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .Build()
                                                    .RunAsync();
        }
    }
}
