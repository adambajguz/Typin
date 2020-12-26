namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class InvalidOptionShortNameCommand : SelfSerializeCommandBase
    {
        [CommandOption('1')]
        public string? Apples { get; init; }

        [CommandOption("fruits", '0')]
        public string? Oranges { get; init; }
    }
}