namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;
    using Typin.Tests.Data.CustomTypes.InitializableByConverter;

    [Command("cmd")]
    public class InvalidOptionConverterCommand : SelfSerializeCommandBase
    {
        [CommandOption("str-nullable-struct", Converter = typeof(object))]
        public InitializableStructTypeByConverter? StringNullableInitializableStruct { get; init; }

        public InvalidOptionConverterCommand(IConsole console) : base(console)
        {

        }
    }
}