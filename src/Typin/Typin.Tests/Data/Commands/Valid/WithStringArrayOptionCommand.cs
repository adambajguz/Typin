namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class WithStringArrayOptionCommand : SelfSerializeCommandBase
    {
        [CommandOption("opt", 'o')]
        public IReadOnlyList<string>? Opt { get; init; }

        public WithStringArrayOptionCommand(IConsole console) : base(console)
        {

        }
    }
}