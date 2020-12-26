namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class SingleCharacterOptionNameCommand : SelfSerializeCommandBase
    {
        [CommandOption("a")]
        public string? Apples { get; init; }
    }
}