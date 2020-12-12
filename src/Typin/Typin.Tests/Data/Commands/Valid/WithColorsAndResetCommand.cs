namespace Typin.Tests.Data.Commands.Valid
{
    using System;
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

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.ResetColor();

            console.WithBackgroundColor(ConsoleColor.Magenta, (c) => c.Output.WriteLine("Magenta"));
            console.WithForegroundColor(ConsoleColor.Green, (c) => c.Output.WriteLine("Green"));
            console.WithColors(ConsoleColor.Red, ConsoleColor.Yellow, (c) => c.Output.Write("Red"));

            console.Output.WriteLine(nameof(WithColorsAndResetCommand));

            return default;
        }
    }
}