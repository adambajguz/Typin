namespace Typin.Tests.Data.Valid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command(nameof(WithEnumArgumentsCommand))]
    public class WithEnumArgumentsCommand : SelfSerializeCommandBase
    {
        public WithEnumArgumentsCommand(IConsole console) : base(console)
        {

        }

        public enum CustomEnum { Value1, Value2, Value3 };

        [Parameter(0, Name = "enum")]
        public CustomEnum EnumParameter { get; init; }

        [Option("enum")]
        public CustomEnum? EnumOption { get; init; }

        [Option("arr-enum")]
        public CustomEnum?[] ArrayEnumOption { get; init; } = default!;

        [Option("required-enum", IsRequired = true)]
        public CustomEnum RequiredEnumOption { get; init; }
    }
}