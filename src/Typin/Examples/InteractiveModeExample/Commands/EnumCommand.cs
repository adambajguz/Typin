namespace InteractiveModeExample.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("enum", Description = "Enum test command.")]
    public class EnumCommand : ICommand
    {
        [CommandOption("d1")]
        public DayOfWeek Day1 { get; init; }

        [CommandOption("d2")]
        public DayOfWeek Day2 { get; init; } = DayOfWeek.Friday;

        [CommandOption("d3")]
        public List<DayOfWeek> Day3 { get; init; } = default!;

        [CommandOption("d4")]
        public IEnumerable<DayOfWeek> Day4 { get; init; } = default!;

        public ValueTask ExecuteAsync(IConsole console)
        {
            return default;
        }
    }
}
