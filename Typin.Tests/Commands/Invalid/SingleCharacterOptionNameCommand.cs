namespace Typin.Tests.Commands.Invalid
{
    using Typin.Attributes;

    [Command("cmd")]
    public class SingleCharacterOptionNameCommand : SelfSerializeCommandBase
    {
        [CommandOption("a")]
        public string? Apples { get; set; }
    }
}