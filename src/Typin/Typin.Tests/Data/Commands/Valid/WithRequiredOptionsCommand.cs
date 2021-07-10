namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithRequiredOptionsCommand : SelfSerializeCommandBase
    {
        [CommandOption("opt-a", 'a', IsRequired = true)]
        public string? OptA { get; init; }

        [CommandOption("opt-b", 'b')]
        public int? OptB { get; init; }

        [CommandOption("opt-c", 'c', IsRequired = true)]
        public IReadOnlyList<char>? OptC { get; init; }

        public WithRequiredOptionsCommand(IConsole console) : base(console)
        {

        }
    }
}