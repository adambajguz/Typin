namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;
    using Typin.Tests.Data.CustomTypes.InitializableByConverter;

    [Command("cmd")]
    public class SupportedArgumentTypesViaConverterCommand : SelfSerializeCommandBase
    {
        [Option("str-class", Converter = typeof(InitializableClassTypeByConverter_Converter))]
        public InitializableClassTypeByConverter? StringInitializable { get; init; }

        [Option("str-nullable-struct", Converter = typeof(InitializableNullableStructTypeByConverter_Converter))]
        public InitializableStructTypeByConverter? StringNullableInitializableStruct { get; init; }

        [Option("str-nullable-struct-by-non-nullable-converter", Converter = typeof(InitializableStructTypeByConverter_Converter))]
        public InitializableStructTypeByConverter? StringNullableInitializableStructByNonNullableConverter { get; init; }

        [Option("str-struct", Converter = typeof(InitializableStructTypeByConverter_Converter))]
        public InitializableStructTypeByConverter StringInitializableStruct { get; init; }

        [Option("str-enumerable", Converter = typeof(InitializableEnumerableByConverter_Converter<string>))]
        public InitializableEnumerableByConverter<string>? StringEnumerableInitializable { get; init; }

        [Option("str-indirect-enumerable", Converter = typeof(InitializableEnumerableByConverter_Converter<string>))]
        public InitializableEnumerableByConverter? IndirectlyStringEnumerableInitializable { get; init; }

        public SupportedArgumentTypesViaConverterCommand(IConsole console) : base(console)
        {

        }
    }
}