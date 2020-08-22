namespace Typin.Tests.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;

    [Command("cmd")]
    public class WithEnvironmentVariablesCommand : SelfSerializeCommandBase
    {
        [CommandOption("opt-a", 'a', EnvironmentVariableName = "ENV_OPT_A")]
        public string? OptA { get; set; }

        [CommandOption("opt-b", 'b', EnvironmentVariableName = "ENV_OPT_B")]
        public IReadOnlyList<string>? OptB { get; set; }
    }
}