﻿namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class DuplicateParameterOrderCommand : SelfSerializeCommandBase
    {
        [CommandParameter(13)]
        public string? ParamA { get; init; }

        [CommandParameter(13)]
        public string? ParamB { get; init; }
    }
}