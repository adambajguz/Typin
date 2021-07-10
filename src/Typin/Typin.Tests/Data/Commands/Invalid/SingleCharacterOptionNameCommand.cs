namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class SingleCharacterOptionNameCommand : SelfSerializeCommandBase
    {
        [CommandOption("a")]
        public string? Apples { get; init; }

        public SingleCharacterOptionNameCommand(IConsole console) : base(console)
        {

        }
    }
}