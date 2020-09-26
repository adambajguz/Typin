namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithEnvironmentVariablesCommand : SelfSerializeCommandBase
    {
        [CommandOption("opt-a", 'a', FallbackVariableName = "ENV_OPT_A")]
        public string? OptA { get; set; }

        [CommandOption("opt-b", 'b', FallbackVariableName = "ENV_OPT_B")]
        public IReadOnlyList<string>? OptB { get; set; }
    }
}