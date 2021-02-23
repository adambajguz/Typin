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
        public object? Object { get; init; } = 42;

        [CommandOption]
        public string? String { get; init; } = "foo";

        [CommandOption]
        public string StringEmpty { get; init; } = "";

        [CommandOption]
        public string[]? StringArray { get; init; } = { "foo", "bar", "baz" };

        [CommandOption]
        public bool Bool { get; init; } = true;

        [CommandOption]
        public char Char { get; init; } = 't';

        [CommandOption]
        public int Int { get; init; } = 1337;

        [CommandOption]
        public int? IntNullable { get; init; } = 1337;

        [CommandOption]
        public int[]? IntArray { get; init; } = { 1, 2, 3 };

        [CommandOption]
        public TimeSpan TimeSpan { get; init; } = TimeSpan.FromMinutes(123);

        [CommandOption]
        public CustomEnum Enum { get; init; } = CustomEnum.Value2;
    }
}