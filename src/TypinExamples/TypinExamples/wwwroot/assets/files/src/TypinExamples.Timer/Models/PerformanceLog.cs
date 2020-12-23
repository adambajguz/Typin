namespace TypinExamples.Timer.Models
{
    using System;
    using System.Collections.Generic;

    public record PerformanceLog
    {
        public int Id { get; init; }
        public DateTime StartedOn { get; init; }
        public string? CommandName { get; init; }
        public IReadOnlyList<string>? Input { get; init; }
        public TimeSpan Time { get; init; }
    }
}
