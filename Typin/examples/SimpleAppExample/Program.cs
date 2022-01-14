namespace SimpleAppExample
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using SimpleAppExample.Commands;
    using SimpleAppExample.Directives;
    using Typin.Commands;
    using Typin.Directives;
    using Typin.Hosting;
    using Typin.Models;
    using Typin.Modes;

    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members")]
    public static class Program
    {
        private static readonly string[] Arguments = { "-125", "--req-str", "welcome", "--str", "hello world", "-i", "-13", "-b" };
        private static readonly string[] ArgumentsWithHelp = { "-125", "--req-str", "welcome", "--str", "hello world", "-i", "-13", "-b", "--help" };
        private static readonly string[] ArgumentsWithDirectives = { "[chelp]", "[cpreview:", "name", "--delay", "2000]", "--req-str", "welcome", "-125", "--str", "hello world", "-i", "-13", "-b" };

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
                        .ConfigureModels(scanner =>
                        {
                            scanner.Single(typeof(ConfigureModelsFromAttributes));
                            scanner.Single(typeof(ConfigureModelFromAttributes<>));
                        })
                        .ConfigureCommands(scanner =>
                        {
                            scanner.Single(typeof(ConfigureCommandsFromAttributes));
                            scanner.Single(typeof(ConfigureCommandFromAttributes<>));
                        })
                        .AddCommands(scanner =>
                        {
                            scanner.FromThisAssembly();
                        })
                        .AddDirectives(scanner =>
                        {
                            scanner.Single<CustomHelpDirective>()
                                   .Single<CustomPreviewDirective>();
                        })
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                        });

                    cliBuilder.OverrideCommandLine(ArgumentsWithDirectives)
                              .UseStartupMessage("Welcome!")
                              .SetStartupMode<DirectMode>();
                })
                .RunConsoleAsync();

            return 0;
        }
    }
}