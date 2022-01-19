namespace InteractiveModeExample.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;

    [Alias("enum")]
    public class EnumCommand : ICommand
    {
        [Option("d1")]
        public DayOfWeek Day1 { get; init; }

        [Option("d2")]
        public DayOfWeek Day2 { get; init; } = DayOfWeek.Friday;

        [Option("d3")]
        public List<DayOfWeek> Day3 { get; init; } = default!;

        [Option("d4")]
        public IEnumerable<DayOfWeek> Day4 { get; init; } = default!;

        [Option("d1N")]
        public DayOfWeek? Day1N { get; init; }

        [Option("d2N")]
        public DayOfWeek? Day2N { get; init; } = DayOfWeek.Friday;

        [Option("d2NN", Description = "Nullable enum.")]
        public DayOfWeek? Day2NN { get; init; } = null;

        [Option("d3N", Description = "Days.")]
        public List<DayOfWeek?> Day3N { get; init; } = default!;

        [Option("d4N")]
        public IEnumerable<DayOfWeek?> Day4N { get; init; } = default!;

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
