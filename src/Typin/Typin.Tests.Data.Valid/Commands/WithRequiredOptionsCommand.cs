namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command(nameof(WithRequiredOptionsCommand))]
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