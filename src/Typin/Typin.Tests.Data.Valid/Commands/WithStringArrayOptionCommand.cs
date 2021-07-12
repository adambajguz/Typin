namespace Typin.Tests.Data.Commands.Valid
{
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Console;
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