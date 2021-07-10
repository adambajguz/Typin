namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;
    using Typin.Tests.Data.CustomTypes.InitializableByConverter;

    [Command("cmd")]
    public class InvalidParameterConverterCommand : SelfSerializeCommandBase
    {
        [CommandParameter(0, Converter = typeof(object))]
        public InitializableClassTypeByConverter? StringInitializable { get; init; }

        [CommandOption("str-nullable-struct", Converter = typeof(InitializableNullableStructTypeByConverter_Converter))]
        public InitializableStructTypeByConverter? StringNullableInitializableStruct { get; init; }

        public InvalidParameterConverterCommand(IConsole console) : base(console)
        {

        }
    }
}