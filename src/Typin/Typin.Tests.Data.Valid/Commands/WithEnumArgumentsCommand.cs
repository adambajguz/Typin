namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command(nameof(WithEnumArgumentsCommand))]
    public class WithEnumArgumentsCommand : SelfSerializeCommandBase
    {
        public enum CustomEnum { Value1, Value2, Value3 };

        [Parameter(0, Name = "enum")]
        public CustomEnum EnumParameter { get; init; }

        [Option("enum")]
        public CustomEnum? EnumOption { get; init; }

        [Option("required-enum", IsRequired = true)]
        public CustomEnum RequiredEnumOption { get; init; }

        public WithEnumArgumentsCommand(IConsole console) : base(console)
        {

        }
    }
}