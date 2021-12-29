namespace Typin.Tests.Data.Invalid.Commands
{
    using Typin.Commands.Attributes;
    using Typin.Console;
    using Typin.Models.Attributes;
    using Typin.Tests.Data.Common.Commands;

    [Command("cmd")]
    public class SingleCharacterOptionNameCommand : SelfSerializeCommandBase
    {
        [Option("a")]
        public string? Apples { get; init; }

        public SingleCharacterOptionNameCommand(IConsole console) : base(console)
        {

        }
    }
}