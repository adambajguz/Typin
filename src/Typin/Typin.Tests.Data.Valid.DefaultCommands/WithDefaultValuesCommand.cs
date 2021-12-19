namespace Typin.Tests.Data.Valid.DefaultCommands
{
    using System;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class WithDefaultValuesCommand : SelfSerializeCommandBase
    {
        public enum CustomEnum { Value1, Value2, Value3 };

        [Option("obj")]
        public object? Object { get; init; } = 42;

        [Option("str")]
        public string? String { get; init; } = "foo";

        [Option("str-empty")]
        public string StringEmpty { get; init; } = "";

        [Option("str-array")]
        public string[]? StringArray { get; init; } = { "foo", "bar", "baz" };

        [Option("bool")]
        public bool Bool { get; init; } = true;

        [Option("char")]
        public char Char { get; init; } = 't';

        [Option("int")]
        public int Int { get; init; } = 1337;

        [Option("int-nullable")]
        public int? IntNullable { get; init; } = 1337;

        [Option("int-array")]
        public int[]? IntArray { get; init; } = { 1, 2, 3 };

        [Option("timespan")]
        public TimeSpan TimeSpan { get; init; } = TimeSpan.FromMinutes(123);

        [Option("enum")]
        public CustomEnum Enum { get; init; } = CustomEnum.Value2;

        public WithDefaultValuesCommand(IConsole console) : base(console)
        {

        }
    }
}