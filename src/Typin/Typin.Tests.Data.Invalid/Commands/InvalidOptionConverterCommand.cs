﻿namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;
    using Typin.Tests.Data.Common.CustomTypes.InitializableByConverter;

    [Command("cmd")]
    public class InvalidOptionConverterCommand : SelfSerializeCommandBase
    {
        [Option("str-nullable-struct", Converter = typeof(object))]
        public InitializableStructTypeByConverter? StringNullableInitializableStruct { get; init; }

        public InvalidOptionConverterCommand(IConsole console) : base(console)
        {

        }
    }
}