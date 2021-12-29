namespace Typin.Tests.Data.Valid.Commands
{
    using System.Collections.Generic;
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command(nameof(WithEnvironmentVariablesCommand))]
    public class WithEnvironmentVariablesCommand : SelfSerializeCommandBase
    {
        [Option("opt-a", 'a', FallbackVariableName = "ENV_OPT_A")]
        public string? OptA { get; init; }

        [Option("opt-b", 'b', FallbackVariableName = "ENV_OPT_B")]
        public IReadOnlyList<string>? OptB { get; init; }

        public WithEnvironmentVariablesCommand(IConsole console) : base(console)
        {

        }
    }
}