﻿namespace Typin.Tests.Data.Commands.Valid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithEnumArgumentsCommand : SelfSerializeCommandBase
    {
        public enum CustomEnum { Value1, Value2, Value3 };

        [CommandParameter(0, Name = "enum")]
        public CustomEnum EnumParameter { get; init; }

        [CommandOption("enum")]
        public CustomEnum? EnumOption { get; init; }

        [CommandOption("arr-enum")]
        public CustomEnum?[] ArrayEnumOption { get; init; } = default!;

        [CommandOption("required-enum", IsRequired = true)]
        public CustomEnum RequiredEnumOption { get; init; }
    }
}