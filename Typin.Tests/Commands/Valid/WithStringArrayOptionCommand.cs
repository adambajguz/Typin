namespace Typin.Tests.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;

    [Command("cmd")]
    public class WithStringArrayOptionCommand : SelfSerializeCommandBase
    {
        [CommandOption("opt", 'o')]
        public IReadOnlyList<string>? Opt { get; set; }
    }
}