namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;
    using Typin.Tests.Data.CustomTypes.NonInitializable;

    [Command("cmd")]
    public class UnsupportedArgumentTypesCommand : SelfSerializeCommandBase
    {
        [CommandOption("str-non-initializable-class")]
        public NonInitializableClassType? StringNonInitializable { get; set; }

        [CommandOption("str-non-initializable-struct")]
        public NonInitializableStructType? StringNonInitializableStruct { get; set; }

        [CommandOption("str-enumerable-non-initializable")]
        public NonInitializableEnumerable<string>? StringEnumerableNonInitializable { get; set; }
    }
}