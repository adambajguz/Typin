namespace Typin.Tests.Data.Valid.Commands
{
    using System.Collections.Generic;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Schemas.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Alias(nameof(WithRequiredOptionsCommand))]
    public class WithRequiredOptionsCommand : SelfSerializeCommandBase
    {
        [Option("opt-a", 'a', IsRequired = true)]
        public string? OptA { get; init; }

        [Option("opt-b", 'b')]
        public int? OptB { get; init; }

        [Option("opt-c", 'c', IsRequired = true)]
        public IReadOnlyList<char>? OptC { get; init; }

        public WithRequiredOptionsCommand(IConsole console) : base(console)
        {

        }
    }
}