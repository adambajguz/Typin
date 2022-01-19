﻿namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class InvalidOptionShortNameCommand : SelfSerializeCommandBase
    {
        [Option('1')]
        public string? Apples { get; init; }

        [Option("fruits", '0')]
        public string? Oranges { get; init; }

        public InvalidOptionShortNameCommand(IConsole console) : base(console)
        {

        }
    }
}