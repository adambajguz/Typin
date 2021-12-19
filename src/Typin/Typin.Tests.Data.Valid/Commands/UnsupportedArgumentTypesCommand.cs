namespace Typin.Tests.Data.Valid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;
    using Typin.Tests.Data.Common.CustomTypes.NonInitializable;

    [Command(nameof(UnsupportedArgumentTypesCommand))]
    public class UnsupportedArgumentTypesCommand : SelfSerializeCommandBase
    {
        [Option("str-non-initializable-class")]
        public NonInitializableClassType? StringNonInitializable { get; init; }

        [Option("str-non-initializable-struct")]
        public NonInitializableStructType? StringNonInitializableStruct { get; init; }

        [Option("str-enumerable-non-initializable")]
        public NonInitializableEnumerable<string>? StringEnumerableNonInitializable { get; init; }

        public UnsupportedArgumentTypesCommand(IConsole console) : base(console)
        {

        }
    }
}