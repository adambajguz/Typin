namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class EmptyOptionNameCommand : SelfSerializeCommandBase
    {
        [CommandOption("")]
        public string? Apples { get; set; }
    }
}