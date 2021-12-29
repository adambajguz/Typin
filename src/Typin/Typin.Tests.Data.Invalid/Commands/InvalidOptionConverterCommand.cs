namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;
    using Typin.Tests.Data.Common.CustomTypes.InitializableByConverter;

    [Command(nameof(InvalidOptionConverterCommand))]
    public class InvalidOptionConverterCommand : SelfSerializeCommandBase
    {
        [Option("str-nullable-struct", Converter = typeof(object))]
        public InitializableStructTypeByConverter? StringNullableInitializableStruct { get; init; }

        public InvalidOptionConverterCommand(IConsole console) : base(console)
        {

        }
    }
}