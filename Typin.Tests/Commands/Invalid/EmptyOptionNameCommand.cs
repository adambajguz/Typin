namespace Typin.Tests.Commands.Invalid
{
    using Typin.Attributes;

    [Command("cmd")]
    public class EmptyOptionNameCommand : SelfSerializeCommandBase
    {
        [CommandOption("")]
        public string? Apples { get; set; }
    }
}