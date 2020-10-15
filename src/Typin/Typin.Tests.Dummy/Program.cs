namespace Typin.Tests.Dummy
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class Program
    {
        public static Assembly Assembly { get; } = typeof(Program).Assembly;

        public static string Location { get; } = Assembly.Location;

        [ExcludeFromCodeCoverage]
        public static async Task Main()
        {
            await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync();
        }
    }
}