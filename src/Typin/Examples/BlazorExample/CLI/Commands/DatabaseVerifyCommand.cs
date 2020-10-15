namespace BlazorExample.CLI.Commands
{
    using System;
    using System.Threading.Tasks;
    using ShellProgressBar;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Utilities;

    [Command("database verify", Description = "Migrates the database.")]
    public class DatabaseVerifyCommand : ICommand
    {
        public DatabaseVerifyCommand()
        {

        }

        private bool RequestToQuit { get; set; }

        private void TickToCompletion(IConsole console, IProgressBar pbar, int ticks, int sleep = 1750, Action<int>? childAction = null)
        {
            var initialMessage = pbar.Message;
            for (var i = 0; i < ticks && !RequestToQuit; i++)
            {
                pbar.Message = $"Start {i + 1} of {ticks} {Console.CursorTop}/{Console.WindowHeight}: {initialMessage}";
                childAction?.Invoke(i);
                Task.Delay(sleep, console.GetCancellationToken());
                pbar.Tick($"End {i + 1} of {ticks} {Console.CursorTop}/{Console.WindowHeight}: {initialMessage}");
            }
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync("This will normally verify EF Core migrations.");

            // Typin progress
            {
                var progressTicker = console.CreateProgressTicker();
                for (var i = 0.0; i <= 1.01; i += 0.01)
                {
                    progressTicker.Report(i);
                    await Task.Delay(15, console.GetCancellationToken());
                }
                console.Output.WriteLine();
            }

            // ShellProgressBar
            {
                const int totalTicks = 10;
                var options = new ProgressBarOptions
                {
                    ForegroundColor = ConsoleColor.Yellow,
                    BackgroundColor = ConsoleColor.DarkGray,
                    ProgressCharacter = '─'
                };
                var childOptions = new ProgressBarOptions
                {
                    ForegroundColor = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    ProgressCharacter = '\u2593',
                    CollapseWhenFinished = true
                };
                using (var pbar = new ProgressBar(totalTicks, "main progressbar", options))
                    TickToCompletion(console, pbar, totalTicks, sleep: 10, childAction: (i) =>
                    {
                        using (var child = pbar.Spawn(totalTicks, "child actions", childOptions))
                            TickToCompletion(console, child, totalTicks, sleep: 100);
                    });
            }
        }
    }
}
