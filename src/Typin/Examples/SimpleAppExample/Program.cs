namespace SimpleAppExample
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Typin.Commands;
    using Typin.Hosting;
    using Typin.Modes;

    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members")]
    public static class Program
    {
        private static readonly string[] Arguments = { "-125", "--req-str", "welcome", "--str", "hello world", "-i", "-13", "-b" };
        private static readonly string[] ArgumentsWithHelp = { "-125", "--req-str", "welcome", "--str", "hello world", "-i", "-13", "-b", "--help" };
        private static readonly string[] ArgumentsWithPreview = { "[preview]", "--req-str", "welcome",  "-125", "--str", "hello world", "-i", "-13", "-b" };

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
                        //TODO:
                        //.AddDirectives(scanner =>
                        //{
                        //    scanner.Single<DebugDirective>()
                        //           .Single<PreviewDirective>();
                        //})
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                        });

                    cliBuilder.OverrideCommandLine(Arguments)
                              .UseStartupMessage("Welcome!")
                              .SetStartupMode<DirectMode>();
                })
                .RunConsoleAsync();

            return 0;
        }
    }
}