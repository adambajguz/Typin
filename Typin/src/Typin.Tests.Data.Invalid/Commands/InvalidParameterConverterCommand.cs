﻿namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;
    using Typin.Tests.Data.Common.CustomTypes.InitializableByConverter;

    [Command(nameof(InvalidParameterConverterCommand))]
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