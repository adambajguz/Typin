namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;
    using Typin.Tests.Data.CustomTypes.InitializableByConverter;

    [Command("cmd")]
    public class SupportedArgumentTypesViaFailingConverterCommand : SelfSerializeCommandBase
    {
        [CommandOption("str-class", Converter = typeof(InitializableClassTypeByConverter_FailingConverter))]
        public InitializableClassTypeByConverter? StringInitializable { get; init; }

        [CommandOption("str-nullable-struct", Converter = typeof(InitializableNullableStructTypeByConverter_FailingConverter))]
        public InitializableStructTypeByConverter? StringNullableInitializableStruct { get; init; }

        [CommandOption("str-nullable-struct-by-non-nullable-converter", Converter = typeof(InitializableStructTypeByConverter_FailingConverter))]
        public InitializableStructTypeByConverter? StringNullableInitializableStructByNonNullableConverter { get; init; }

        [CommandOption("str-struct", Converter = typeof(InitializableStructTypeByConverter_FailingConverter))]
        public InitializableStructTypeByConverter StringInitializableStruct { get; init; }

        [CommandOption("str-enumerable", Converter = typeof(InitializableEnumerableByConverter_FailingConverter<string>))]
        public InitializableEnumerableByConverter<string>? StringEnumerableInitializable { get; init; }

        [CommandOption("str-indirect-enumerable", Converter = typeof(InitializableEnumerableByConverter_FailingConverter<string>))]
        public InitializableEnumerableByConverter? IndirectlyStringEnumerableInitializable { get; init; }

        public SupportedArgumentTypesViaFailingConverterCommand(IConsole console) : base(console)
        {

        }
    }
}