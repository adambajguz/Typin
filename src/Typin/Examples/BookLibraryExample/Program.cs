namespace BookLibraryExample
{
    using System;
    using System.Threading.Tasks;
    using BookLibraryExample.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Typin.Directives;
    using Typin.Hosting;
    using Typin.Modes;

    public static class Program
    {
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

                            scanner.FromThisAssembly();
                        })
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                            scanner.Single<InteractiveMode>();
                        });

                    cliBuilder.SetStartupMode<InteractiveMode>();

                    cliBuilder.UseStartupMessage((metadata) => $"{metadata.Title} CLI {metadata.VersionText} {metadata.ExecutableName} {metadata.Description}");
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<LibraryService>();

                    services.Configure<InteractiveModeOptions>(options =>
                    {
                        options.PromptForeground = ConsoleColor.Magenta;
                        options.IsAdvancedInputAvailable = false;
                        options.SetPrompt("~$ ");
                    });
                })
                .RunConsoleAsync();

            //.UseMiddleware<ExecutionTimingMiddleware>()

            return 0;
        }
    }
}