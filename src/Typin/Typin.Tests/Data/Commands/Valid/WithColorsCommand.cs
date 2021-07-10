namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Console;

    [Command("colors", Description = "With colors command description")]
    public class WithColorsCommand : ICommand
    {
        public static string ExpectedOutputText { get; } = Ansi.Color.Background.FromConsoleColor(ConsoleColor.Magenta) + "Magenta" + Environment.NewLine + Ansi.Color.Background.FromConsoleColor(ConsoleColor.Black) +
                                                           Ansi.Color.Foreground.FromConsoleColor(ConsoleColor.Green) + "Green" + Environment.NewLine + Ansi.Color.Foreground.FromConsoleColor(ConsoleColor.Gray) +
                                                           Ansi.Color.Foreground.FromConsoleColor(ConsoleColor.Red) + Ansi.Color.Background.FromConsoleColor(ConsoleColor.Yellow) + "Red" + Ansi.Color.Foreground.FromConsoleColor(ConsoleColor.Gray) + Ansi.Color.Background.FromConsoleColor(ConsoleColor.Black) +
                                                           nameof(WithColorsCommand) + Environment.NewLine;

        private readonly IConsole _console;

        public WithColorsCommand(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.ForegroundColor = ConsoleColor.Gray;

            _console.Output.WithBackgroundColor(ConsoleColor.Magenta, (output) => output.WriteLine("Magenta"));
            _console.Output.WithForegroundColor(ConsoleColor.Green, (output) => output.WriteLine("Green"));
            _console.Output.WithColors(ConsoleColor.Red, ConsoleColor.Yellow, (output) => output.Write("Red"));

            _console.Output.WriteLine(nameof(WithColorsCommand));

            return default;
        }
    }
}