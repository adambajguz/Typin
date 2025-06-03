namespace Typin.Tests.Data.Valid.Commands
{
    using System.Collections.Generic;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Alias(nameof(WithEnvironmentVariablesCommand))]
    public class WithEnvironmentVariablesCommand : SelfSerializeCommandBase
    {
        [Option("opt-a", 'a')]
        public string? OptA { get; init; }

        [Option("opt-b", 'b')]
        public IReadOnlyList<string>? OptB { get; init; }

        public WithEnvironmentVariablesCommand(IConsole console) : base(console)
        {

        }
    }
}