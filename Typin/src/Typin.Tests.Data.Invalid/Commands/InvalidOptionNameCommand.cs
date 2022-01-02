namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class InvalidOptionNameCommand : SelfSerializeCommandBase
    {
        [Option("1a")]
        public string? Apples { get; init; }

        [Option("1", 'c')]
        public string? Oranges { get; init; }

        public InvalidOptionNameCommand(IConsole console) : base(console)
        {

        }
    }
}