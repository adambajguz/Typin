namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithRequiredOptionsCommand : SelfSerializeCommandBase
    {
        [CommandOption("opt-a", 'a', IsRequired = true)]
        public string? OptA { get; set; }

        [CommandOption("opt-b", 'b')]
        public int? OptB { get; set; }

        [CommandOption("opt-c", 'c', IsRequired = true)]
        public IReadOnlyList<char>? OptC { get; set; }
    }
}