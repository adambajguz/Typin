namespace Typin.Tests.Data.Valid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;
    using Typin.Tests.Data.Common.CustomTypes.InitializableByConverter;

    [Command(nameof(SupportedArgumentTypesViaFailingConverterCommand))]
    public class SupportedArgumentTypesViaFailingConverterCommand : SelfSerializeCommandBase
    {
        [Option("str-class", Converter = typeof(InitializableClassTypeByConverter_FailingConverter))]
        public InitializableClassTypeByConverter? StringInitializable { get; init; }

        [Option("str-nullable-struct", Converter = typeof(InitializableNullableStructTypeByConverter_FailingConverter))]
        public InitializableStructTypeByConverter? StringNullableInitializableStruct { get; init; }

        [Option("str-nullable-struct-by-non-nullable-converter", Converter = typeof(InitializableStructTypeByConverter_FailingConverter))]
        public InitializableStructTypeByConverter? StringNullableInitializableStructByNonNullableConverter { get; init; }

        [Option("str-struct", Converter = typeof(InitializableStructTypeByConverter_FailingConverter))]
        public InitializableStructTypeByConverter StringInitializableStruct { get; init; }

        [Option("str-enumerable", Converter = typeof(InitializableEnumerableByConverter_FailingConverter<string>))]
        public InitializableEnumerableByConverter<string>? StringEnumerableInitializable { get; init; }

        [Option("str-indirect-enumerable", Converter = typeof(InitializableEnumerableByConverter_FailingConverter<string>))]
        public InitializableEnumerableByConverter? IndirectlyStringEnumerableInitializable { get; init; }

        public SupportedArgumentTypesViaFailingConverterCommand(IConsole console) : base(console)
        {

        }
    }
}