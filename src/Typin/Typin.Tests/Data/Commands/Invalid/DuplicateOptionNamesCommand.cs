namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;

    [Command("cmd")]
    public class DuplicateOptionNamesCommand : SelfSerializeCommandBase
    {
        [CommandOption("fruits")]
        public string? Apples { get; init; }

        [CommandOption("fruits")]
        public string? Oranges { get; init; }
    }
}