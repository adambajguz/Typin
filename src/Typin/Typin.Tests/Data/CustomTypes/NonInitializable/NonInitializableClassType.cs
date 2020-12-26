namespace Typin.Tests.Data.CustomTypes.NonInitializable
{
    using System;

    public class NonInitializableClassType
    {
        public int Value { get; init; }
        public DayOfWeek Day { get; init; }
    }
}