namespace Typin.Tests.Dummy
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Typin.Commands;
    using Typin.Hosting;
    using Typin.Modes;

    public static class Program
    {
        public static Assembly Assembly { get; } = typeof(Program).Assembly;

        public static string Location { get; } = Assembly.Location;

        public static async Task<int> Main()
        {
            await CliHost.CreateDefaultBuilder()
                .ConfigureLogging((context, builder) =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureCliHost((cliBuilder) =>
                {
                    cliBuilder
                        .AddCommands(scanner =>
                        {
                            scanner.FromThisAssembly();
                        })
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                        });

                    cliBuilder.SetStartupMode<DirectMode>();
                })
                .RunConsoleAsync();

            return 0;
        }
    }
}