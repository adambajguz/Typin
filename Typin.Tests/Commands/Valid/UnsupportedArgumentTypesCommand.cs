namespace Typin.Tests.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Tests.Commands.CustomTypes.NonInitializable;

    [Command("cmd")]
    public partial class UnsupportedArgumentTypesCommand : SelfSerializeCommandBase
    {
        [CommandOption("str-non-initializable")]
        public NonInitializableClassType? StringNonInitializable { get; set; }

        [CommandOption("str-non-initializable")]
        public NonInitializableStructType? StringNonInitializableStruct { get; set; }

        [CommandOption("str-enumerable-non-initializable")]
        public NonInitializableEnumerable<string>? StringEnumerableNonInitializable { get; set; }
    }
}