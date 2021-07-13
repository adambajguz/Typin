﻿namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    // Must be default because version option is available only on default commands
    [Command]
    public class ConflictWithVersionOptionCommand : SelfSerializeCommandBase
    {
        [Option("version")]
        public string? Version { get; init; }

        public ConflictWithVersionOptionCommand(IConsole console) : base(console)
        {

        }
    }
}