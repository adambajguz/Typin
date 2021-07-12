namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Console;

    [Command("colors-with-reset", Description = "With colors command description")]
    public class WithColorsAndResetCommand : ICommand
    {
        public static string ExpectedOutputText { get; } = Ansi.Color.Background.FromConsoleColor(ConsoleColor.Magenta) + "Magenta" + Environment.NewLine + Ansi.Color.Background.FromConsoleColor(ConsoleColor.Black) +
                                                           Ansi.Color.Foreground.FromConsoleColor(ConsoleColor.Green) + "Green" + Environment.NewLine + Ansi.Color.Foreground.FromConsoleColor(ConsoleColor.White) +
                                                           Ansi.Color.Foreground.FromConsoleColor(ConsoleColor.Red) + Ansi.Color.Background.FromConsoleColor(ConsoleColor.Yellow) + "Red" + Ansi.Color.Foreground.FromConsoleColor(ConsoleColor.White) + Ansi.Color.Background.FromConsoleColor(ConsoleColor.Black) +
                                                           nameof(WithColorsAndResetCommand) + Environment.NewLine;

        private readonly IConsole _console;

        public WithColorsAndResetCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.ResetColor();

            _console.WithBackgroundColor(ConsoleColor.Magenta, (c) => c.Output.WriteLine("Magenta"));
            _console.WithForegroundColor(ConsoleColor.Green, (c) => c.Output.WriteLine("Green"));
            _console.WithColors(ConsoleColor.Red, ConsoleColor.Yellow, (c) => c.Output.Write("Red"));

            _console.Output.WriteLine(nameof(WithColorsAndResetCommand));

            return default;
        }
    }
}