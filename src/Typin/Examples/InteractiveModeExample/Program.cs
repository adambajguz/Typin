namespace InteractiveModeExample
{
    using System;
    using System.Threading.Tasks;
    using InteractiveModeExample.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Typin.Commands;
    using Typin.Hosting;
    using Typin.Modes;
    using Typin.Modes.Interactive;
    using Typin.Modes.Interactive.Commands;
    using Typin.Modes.Programmatic;

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
                            scanner.Single<InteractiveCommand>();

                            scanner.FromThisAssembly();
                        })
                        //.AddDirectives(scanner =>
                        //{
                        //    scanner.Single<DebugDirective>()
                        //           .Single<PreviewDirective>();

                        //    scanner.Single<InteractiveDirective>();

                        //    scanner.FromThisAssembly();
                        //})
                        .AddModes(scanner =>
                        {
                            scanner.Single<DirectMode>();
                            scanner.Single<InteractiveMode>();
                            scanner.Single<ProgrammaticMode>();
                        });

                    cliBuilder.SetStartupMode<DirectMode>();

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