namespace Typin.Tests.Data.Valid.DefaultCommands
{
    using System;
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class WithDefaultValuesAndNamesCommand : SelfSerializeCommandBase
    {
        public enum CustomEnum { Value1, Value2, Value3 };

        [Option]
        public object? Object { get; init; } = 42;

        [Option]
        public string? String { get; init; } = "foo";

        [Option]
        public string StringEmpty { get; init; } = "";

        [Option]
        public string[]? StringArray { get; init; } = { "foo", "bar", "baz" };

        [Option]
        public bool Bool { get; init; } = true;

        [Option]
        public char Char { get; init; } = 't';

        [Option]
        public int Int { get; init; } = 1337;

        [Option]
        public int? IntNullable { get; init; } = 1337;

        [Option]
        public int[]? IntArray { get; init; } = { 1, 2, 3 };

        [Option]
        public TimeSpan TimeSpan { get; init; } = TimeSpan.FromMinutes(123);

        [Option]
        public CustomEnum Enum { get; init; } = CustomEnum.Value2;

        public WithDefaultValuesAndNamesCommand(IConsole console) : base(console)
        {

        }
    }
}