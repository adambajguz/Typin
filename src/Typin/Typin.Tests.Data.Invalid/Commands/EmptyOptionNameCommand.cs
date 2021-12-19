﻿namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class EmptyOptionNameCommand : SelfSerializeCommandBase
    {
        [Option]
        public string? Apples { get; init; }

        [Option]
        public string? Blackberries { get; init; }

        [Option]
        public string? WestIndianCherry { get; init; }

        [Option]
        public string? CoconutMeat_Or_PitayaDragonfruit { get; init; }

        [Option]
        public string? CoconutMeat_or_Pitaya { get; init; }

        public EmptyOptionNameCommand(IConsole console) : base(console)
        {

        }
    }
}