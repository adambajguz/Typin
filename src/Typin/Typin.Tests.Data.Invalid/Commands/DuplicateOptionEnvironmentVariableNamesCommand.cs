﻿namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class DuplicateOptionEnvironmentVariableNamesCommand : SelfSerializeCommandBase
    {
        [Option("option-a", FallbackVariableName = "ENV_VAR")]
        public string? OptionA { get; init; }

        [Option("option-b", FallbackVariableName = "ENV_VAR")]
        public string? OptionB { get; init; }

        public DuplicateOptionEnvironmentVariableNamesCommand(IConsole console) : base(console)
        {

        }
    }
}