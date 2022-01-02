namespace Typin.Tests.Data.Valid.Commands
{
    using System.Collections.Generic;
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command(nameof(WithStringArrayOptionCommand))]
    public class WithStringArrayOptionCommand : SelfSerializeCommandBase
    {
        [Option("opt", 'o')]
        public IReadOnlyList<string>? Opt { get; init; }

        public WithStringArrayOptionCommand(IConsole console) : base(console)
        {

        }
    }
}