namespace Typin.Tests.Data.Common.CustomTypes.InitializableByConverter
{
    using System;

    public readonly struct InitializableStructTypeByConverter
    {
        public int Value { get; init; }
        public DayOfWeek Day { get; init; }
    }
}