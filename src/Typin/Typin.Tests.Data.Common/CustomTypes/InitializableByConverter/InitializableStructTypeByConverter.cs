namespace Typin.Tests.Data.CustomTypes.InitializableByConverter
{
    using System;

    public struct InitializableStructTypeByConverter
    {
        public int Value { get; init; }
        public DayOfWeek Day { get; init; }
    }
}