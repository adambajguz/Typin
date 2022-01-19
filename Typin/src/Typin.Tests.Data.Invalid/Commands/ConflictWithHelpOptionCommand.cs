﻿namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class ConflictWithHelpOptionCommand : SelfSerializeCommandBase
    {
        [Option("option-h", 'h')]
        public string? OptionH { get; init; }

        public ConflictWithHelpOptionCommand(IConsole console) : base(console)
        {

        }
    }
}