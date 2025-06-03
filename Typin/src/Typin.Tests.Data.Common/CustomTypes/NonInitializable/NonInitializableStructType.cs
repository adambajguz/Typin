namespace Typin.Tests.Data.Common.CustomTypes.NonInitializable
{
    using System;

    public readonly struct NonInitializableStructType
    {
        public int Value { get; init; }
        public DayOfWeek Day { get; init; }
    }
}