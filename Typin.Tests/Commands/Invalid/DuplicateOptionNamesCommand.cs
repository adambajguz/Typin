namespace Typin.Tests.Commands.Invalid
{
    using Typin.Attributes;

    [Command("cmd")]
    public class DuplicateOptionNamesCommand : SelfSerializeCommandBase
    {
        [CommandOption("fruits")]
        public string? Apples { get; set; }

        [CommandOption("fruits")]
        public string? Oranges { get; set; }
    }
}