namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;
    using Typin.Tests.Data.CustomTypes.InitializableByConverter;

    [Command("cmd")]
    public class InvalidParameterConverterCommand : SelfSerializeCommandBase
    {
        [Parameter(0, Converter = typeof(object))]
        public InitializableClassTypeByConverter? StringInitializable { get; init; }

        [Option("str-nullable-struct", Converter = typeof(InitializableNullableStructTypeByConverter_Converter))]
        public InitializableStructTypeByConverter? StringNullableInitializableStruct { get; init; }

        public InvalidParameterConverterCommand(IConsole console) : base(console)
        {

        }
    }
}