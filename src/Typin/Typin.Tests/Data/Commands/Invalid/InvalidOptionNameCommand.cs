namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class InvalidOptionNameCommand : SelfSerializeCommandBase
    {
        [CommandOption("1a")]
        public string? Apples { get; init; }

        [CommandOption("1", 'c')]
        public string? Oranges { get; init; }
    }
}