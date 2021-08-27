namespace SimpleAppExample
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Typin.Directives;
    using Typin.Hosting;

    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
    public static class Program
    {
        private static readonly string[] Arguments = { "-125", "--str", "hello world", "-i", "-13", "-b", "-vx" };
        private static readonly string[] ArgumentsWithHelp = { "-125", "--str", "hello world", "-i", "-13", "-b", "-vx", "--help" };
        private static readonly string[] ArgumentsWithPreview = { "[preview]", "-125", "--str", "hello world", "-i", "-13", "-b", "-vx" };

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
                        .AddDirectives(scanner =>
                        {
                            scanner.Single<DebugDirective>()
                                   .Single<PreviewDirective>();
                        });

                    cliBuilder.OverrideCommandLine(ArgumentsWithHelp)
                              .UseStartupMessage("Application");
                })
                .RunConsoleAsync();

            return 0;
        }
    }
}