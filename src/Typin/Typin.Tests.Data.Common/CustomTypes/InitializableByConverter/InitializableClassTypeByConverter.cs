namespace Typin.Tests.Data.CustomTypes.InitializableByConverter
{
    using System;

    public class InitializableClassTypeByConverter
    {
        public int Value { get; init; }
        public DayOfWeek Day { get; init; }
    }
}