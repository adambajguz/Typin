﻿namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithParametersCommand : SelfSerializeCommandBase
    {
        [Parameter(0)]
        public string? ParamA { get; init; }

        [Parameter(1)]
        public int? ParamB { get; init; }

        [Parameter(2)]
        public IReadOnlyList<string>? ParamC { get; init; }

        public WithParametersCommand(IConsole console) : base(console)
        {

        }
    }
}