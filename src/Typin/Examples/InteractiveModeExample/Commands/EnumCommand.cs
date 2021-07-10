namespace InteractiveModeExample.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;

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

        [CommandOption("d1N")]
        public DayOfWeek? Day1N { get; init; }

        [CommandOption("d2N")]
        public DayOfWeek? Day2N { get; init; } = DayOfWeek.Friday;

        [CommandOption("d2NN")]
        public DayOfWeek? Day2NN { get; init; } = null;

        [CommandOption("d3N")]
        public List<DayOfWeek?> Day3N { get; init; } = default!;

        [CommandOption("d4N")]
        public IEnumerable<DayOfWeek?> Day4N { get; init; } = default!;

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
