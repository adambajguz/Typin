namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
    public class WithDefaultValuesAndNamesCommand : SelfSerializeCommandBase
    {
        public enum CustomEnum { Value1, Value2, Value3 };

        [CommandOption]
        public object? Object { get; set; } = 42;

        [CommandOption]
        public string? String { get; set; } = "foo";

        [CommandOption]
        public string StringEmpty { get; set; } = "";

        [CommandOption]
        public string[]? StringArray { get; set; } = { "foo", "bar", "baz" };

        [CommandOption]
        public bool Bool { get; set; } = true;

        [CommandOption]
        public char Char { get; set; } = 't';

        [CommandOption]
        public int Int { get; set; } = 1337;

        [CommandOption]
        public int? IntNullable { get; set; } = 1337;

        [CommandOption]
        public int[]? IntArray { get; set; } = { 1, 2, 3 };

        [CommandOption]
        public TimeSpan TimeSpan { get; set; } = TimeSpan.FromMinutes(123);

        [CommandOption]
        public CustomEnum Enum { get; set; } = CustomEnum.Value2;
    }
}