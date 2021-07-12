namespace Typin.Tests.Data.CustomTypes.NonInitializable
{
    using System;

    public struct NonInitializableStructType
    {
        public int Value { get; init; }
        public DayOfWeek Day { get; init; }
    }
}