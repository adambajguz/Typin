using System.Threading.Tasks;

namespace Typin.BlazorDemo
{
    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder()
                .UseStartup<CliStartup>()
                .Build()
                .RunAsync();
        }
    }
}
