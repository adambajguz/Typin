namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithDefaultValuesCommand : SelfSerializeCommandBase
    {
        public enum CustomEnum { Value1, Value2, Value3 };

        [CommandOption("obj")]
        public object? Object { get; init; } = 42;

        [CommandOption("str")]
        public string? String { get; init; } = "foo";

        [CommandOption("str-empty")]
        public string StringEmpty { get; init; } = "";

        [CommandOption("str-array")]
        public string[]? StringArray { get; init; } = { "foo", "bar", "baz" };

        [CommandOption("bool")]
        public bool Bool { get; init; } = true;

        [CommandOption("char")]
        public char Char { get; init; } = 't';

        [CommandOption("int")]
        public int Int { get; init; } = 1337;

        [CommandOption("int-nullable")]
        public int? IntNullable { get; init; } = 1337;

        [CommandOption("int-array")]
        public int[]? IntArray { get; init; } = { 1, 2, 3 };

        [CommandOption("timespan")]
        public TimeSpan TimeSpan { get; init; } = TimeSpan.FromMinutes(123);

        [CommandOption("enum")]
        public CustomEnum Enum { get; init; } = CustomEnum.Value2;

        public WithDefaultValuesCommand(IConsole console) : base(console)
        {

        }
    }
}